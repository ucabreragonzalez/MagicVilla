using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Data;

public static class VillaStore
{
    public static List<VillaDto> villaList = new List<VillaDto> {
        new VillaDto {Id=1, Name="Pool View", Occupancy=100, Sqft= 4},
        new VillaDto {Id=2, Name="Beach View", Occupancy=100, Sqft= 3}
    };
}