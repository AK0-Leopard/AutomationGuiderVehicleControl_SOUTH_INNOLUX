﻿// ***********************************************************************
// Assembly         : ScriptControl
// Author           : 
// Created          : 03-31-2016
//
// Last Modified By : 
// Last Modified On : 03-24-2016
// ***********************************************************************
// <copyright file="S5F2.cs" company="">
//     Copyright ©  2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.mirle.ibg3k0.stc.Common;
using com.mirle.ibg3k0.stc.Data.SecsData;

namespace com.mirle.ibg3k0.sc.Data.SECS.SouthInnoluxFactory4
{
    /// <summary>
    /// Class S5F2.
    /// </summary>
    /// <seealso cref="com.mirle.ibg3k0.stc.Data.SecsData.SXFY" />
    public class S5F2 : SXFY
    {
        /// <summary>
        /// The ack c5
        /// </summary>
        //[SecsElement(Index = 1, ListSpreadOut = true, Type = SecsElement.SecsElementType.TYPE_ASCII, Length = 1)]
        [SecsElement(Index = 1, ListSpreadOut = true, Type = SecsElement.SecsElementType.TYPE_BINARY, Length = 1)]//修改Type為TYPE_BINARY MarkChou 20190329
        public string ACKC5;

        /// <summary>
        /// Initializes a new instance of the <see cref="S5F2"/> class.
        /// </summary>
        public S5F2() 
        {
            StreamFunction = "S5F2";
            StreamFunctionName = "Alarm Report Ack";
            W_Bit = 0;
        }
    }
}
