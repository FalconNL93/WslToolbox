﻿using AutoMapper;
using WslToolbox.Core.Legacy;
using WslToolbox.UI.Core.Models;

namespace WslToolbox.UI.Core.Configurations;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<DistributionClass, Distribution>();
        CreateMap<Distribution, DistributionClass>();
    }
}