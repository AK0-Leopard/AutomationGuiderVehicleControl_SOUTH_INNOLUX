//*********************************************************************************
//      BackgroundWorkItem.cs
//*********************************************************************************
// File Name: BackgroundWorkItem.cs
// Description: 背景執行事項的資料集合
//
//(c) Copyright 2014, MIRLE Automation Corporation
//
// Date          Author         Request No.    Tag     Description
// ------------- -------------  -------------  ------  -----------------------------
// 2014/07/06    Hayes Chen     N/A            N/A     Initial Release
//
//**********************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mirle.ibg3k0.ohxc.wpf.Schedule
{
    /// <summary>
    /// Class BackgroundWorkItem.
    /// </summary>
    public class BackgroundWorkItem
    {
        /// <summary>
        /// Gets the parameter.
        /// </summary>
        /// <value>The parameter.</value>
        public Object[] Param { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundWorkItem"/> class.
        /// </summary>
        /// <param name="param">The parameter.</param>
        public BackgroundWorkItem(params Object[] param) 
        {
            Param = param;
        }
    }
}
