using AutoMapper;
using estadoCuentaAPI.DTOs;
using estadoCuentaAPI.Models;


namespace estadoCuentaAPI.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<TarjetaCredito, TarjetaCreditoDTO>().ReverseMap();
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<TarjetaCredito, TarjetaCreditoDTO>()
                .ForMember(dest => dest.FechaCorte,
                    opt => opt.MapFrom(src => src.FechaCorte.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.FechaPago,
                    opt => opt.MapFrom(src => src.FechaPago.ToString("yyyy-MM-dd")))
                .ReverseMap();
            CreateMap<ClienteTarjetaView, ClienteTarjetaDTO>().ReverseMap();
            
            CreateMap<MovimientoTarjetum, MovimientoDTO>()
                .ForMember(dest => dest.FechaMoviomiento,
                    opt => opt.MapFrom(src => src.FechaMovimiento.ToString("yyyy-MM-dd"))).ReverseMap();

            CreateMap<MovimientoTarjetum, MovimientoCompraDTO>().ReverseMap();
            CreateMap<MovimientoTarjetum, PagoDTO>().ReverseMap();
            CreateMap<MovimientoTarjetaDTO, MovimientoCompraDTO>().ReverseMap();
            CreateMap<MovimientoTarjetaDTO, PagoDTO>().ReverseMap();

        }
    }
}
