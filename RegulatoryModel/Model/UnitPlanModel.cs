﻿using System;
using System.Collections.Generic;
using System.Text;

namespace RegulatoryModel.Model
{
    public class UnitPlanModel : ModelBase
    {
        public static string unitPlanNameLayer = "";
        public static string unitPlanLineLayer = "坐标";
        public UnitPlanModel()
        {
            this.IsOnlyModel = false;
            this.specailLayers = new List<string>() { unitPlanNameLayer, unitPlanLineLayer };
            this.DerivedType = DerivedTypeEnum.UnitPlan;
        }
    }
}
