using AutoMapper;
using WslToolbox.Core;
using WslToolbox.Gui2.Models;

namespace WslToolbox.Gui2.Configurations;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<DistributionClass, DistributionModel>();
        CreateMap<DistributionModel, DistributionClass>();
    }
}