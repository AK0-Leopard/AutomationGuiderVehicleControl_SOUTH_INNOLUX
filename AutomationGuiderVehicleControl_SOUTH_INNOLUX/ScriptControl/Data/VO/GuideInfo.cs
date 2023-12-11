using com.mirle.ibg3k0.sc.Common;
using com.mirle.ibg3k0.sc.ProtocolFormat.OHTMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.sc.Data.VO
{
    public class GuideInfo
    {
        public class Section
        {
            public string ID;
            public DriveDirction Dir;
            public bool isPass { get; private set; }
            public void setIsPassFlag()
            {
                isPass = true;
            }
            public Section(string _id, DriveDirction _dir)
            {
                ID = _id;
                Dir = _dir;
                isPass = false;
            }
        }
        public GuideInfo(AVEHICLE _vh)
        {
            vh = _vh;
            startToLoadGuideAddresse = new List<string>();
            startToLoadGuideSection = new List<Section>();
            ToDesinationGuideAddresse = new List<string>();
            ToDesinationGuideSection = new List<Section>();
            AvoidGuideAddresse = new List<string>();
            AvoidGuideSection = new List<Section>();
        }
        public void setGuideSection(sc.BLL.ReserveBLL reserveBLL, ID_31_TRANS_REQUEST id_31, ActiveType originalAactiveType)
        {
            startToLoadGuideAddresse = id_31.GuideAddressesStartToLoad.ToList();
            List<string> startToLoadGuideSectionIDs = id_31.GuideSectionsStartToLoad.ToList();
            startToLoadGuideSection = convertGuideSectionIDToObject(reserveBLL, startToLoadGuideSectionIDs, startToLoadGuideAddresse);
            ToDesinationGuideAddresse = id_31.GuideAddressesToDestination.ToList();
            List<string> ToDesinationGuideSectionIDs = id_31.GuideSectionsToDestination.ToList();
            ToDesinationGuideSection = convertGuideSectionIDToObject(reserveBLL, ToDesinationGuideSectionIDs, ToDesinationGuideAddresse);
            isAvoiding = false;
            judgeIsMoveCommand(id_31, originalAactiveType);
        }

        private void judgeIsMoveCommand(ID_31_TRANS_REQUEST id_31, ActiveType originalAactiveType)
        {
            if (id_31.ActType == ActiveType.Override)
            {
                isMove = originalAactiveType == ActiveType.Move ||
                         originalAactiveType == ActiveType.Movetocharger;
            }
            else
            {
                isMove = id_31.ActType == ActiveType.Move ||
                         id_31.ActType == ActiveType.Movetocharger;
            }
        }

        public void setAvoidSection(sc.BLL.ReserveBLL reserveBLL, ID_51_AVOID_REQUEST id_51)
        {
            AvoidGuideAddresse = id_51.GuideAddresses.ToList();
            List<string> avoidGuideSectionIDs = id_51.GuideSections.ToList();
            AvoidGuideSection = convertGuideSectionIDToObject(reserveBLL, avoidGuideSectionIDs, AvoidGuideAddresse);
            isAvoiding = true;
        }
        private List<Section> convertGuideSectionIDToObject(sc.BLL.ReserveBLL reserveBLL, List<string> guideSectionIDs, List<string> guideAddresses)
        {
            string current_guide_Addresses = string.Join(",", guideAddresses);
            List<Section> GuideSections = new List<Section>();
            foreach (string sec_id in guideSectionIDs)
            {
                //var sec_obj = sectinoBLL.cache.GetSection(sec_id);
                var get_result = reserveBLL.GetHltMapSections(sec_id);
                if (!get_result.isExist)
                {
                    continue;
                }
                string from_to_addresses = $"{SCUtility.Trim(get_result.section.StartAddressID)},{SCUtility.Trim(get_result.section.EndAddressID)}";

                DriveDirction dir = current_guide_Addresses.Contains(from_to_addresses) ?
                                DriveDirction.DriveDirForward : DriveDirction.DriveDirReverse;
                Section sec = new Section(sec_id, dir);
                GuideSections.Add(sec);
            }
            return GuideSections;
        }

        public void setAvoidComplete()
        {
            isAvoiding = false;
        }
        public void resetGuideInfo()
        {
            startToLoadGuideAddresse = new List<string>();
            startToLoadGuideSection = new List<Section>();
            ToDesinationGuideAddresse = new List<string>();
            ToDesinationGuideSection = new List<Section>();
            AvoidGuideAddresse = new List<string>();
            AvoidGuideSection = new List<Section>();
            isMove = false;
            isAvoiding = false;
        }

        AVEHICLE vh;
        bool isAvoiding;
        bool isMove;
        List<string> startToLoadGuideAddresse;
        List<Section> startToLoadGuideSection;
        List<string> ToDesinationGuideAddresse;
        List<Section> ToDesinationGuideSection;
        List<string> AvoidGuideAddresse;
        List<Section> AvoidGuideSection;
        public (bool hasInfo, List<string> currentGuideSection) tryGetCurrentGuideSection()
        {

            var try_get_section_obj_result = tryGetCurrentGuideSectionObj();
            if (try_get_section_obj_result.hasInfo)
            {
                return (true, try_get_section_obj_result.currentGuideSection.Select(sec => sec.ID).ToList());
            }
            else
            {
                return (false, null);
            }

        }
        public (bool hasInfo, List<Section> currentGuideSection) tryGetCurrentGuideSectionObj()
        {
            if (isAvoiding)
            {
                if (AvoidGuideSection != null && AvoidGuideSection.Count > 0)
                    return (true, AvoidGuideSection.ToList());
                else
                    return (false, null);
            }
            else if (isMove)
            {
                if (ToDesinationGuideSection != null && ToDesinationGuideSection.Count > 0)
                    return (true, ToDesinationGuideSection.ToList());
                else
                    return (false, null);
            }
            else
            {
                if (vh.HAS_CST == 0)
                {
                    if (startToLoadGuideSection != null && startToLoadGuideSection.Count > 0)
                        return (true, startToLoadGuideSection.ToList());
                    else
                        return (false, null);
                }
                else
                {
                    if (ToDesinationGuideSection != null && ToDesinationGuideSection.Count > 0)
                        return (true, ToDesinationGuideSection.ToList());
                    else
                        return (false, null);
                }
            }
        }
        public (bool isExist, DriveDirction dir) tryGetWalkDirOnSection(string secID)
        {
            var try_get_current_guide_section_obj_result = tryGetCurrentGuideSectionObj();
            if (!try_get_current_guide_section_obj_result.hasInfo)
                return (false, DriveDirction.DriveDirNone);
            var sec_obj = try_get_current_guide_section_obj_result.currentGuideSection.
                Where(s => SCUtility.isMatche(s.ID, secID)).FirstOrDefault();
            if (sec_obj == null)
            {
                return (false, DriveDirction.DriveDirNone);
            }
            return (true, sec_obj.Dir);
        }
    }

}
