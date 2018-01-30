using System;
using AutoMapper;

namespace Fittify.Api
{
    public static class AutomapperExtensions
    {

        public static void NewMethod(this IMemberConfigurationExpression memberConfigurationExpression)
        {
            // Do something with streamable.
        }

        private static void IgnoreUnmappedProperties(TypeMap map, IMappingExpression expr)
        {
            foreach (string propName in map.GetUnmappedPropertyNames())
            {
                if (map.SourceType.GetProperty(propName) != null)
                {
                    expr.ForSourceMember(propName, opt => opt.Ignore());
                }
                if (map.DestinationType.GetProperty(propName) != null)
                {
                    expr.ForMember(propName, opt => opt.Ignore());
                }
            }
        }

        /// <summary>
        /// Ignores all unmapped members of all maps. Put this method at the end of the config
        /// </summary>
        /// <param name="profile"></param>
        public static void IgnoreUnmapped(this IProfileExpression profile)
        {
            profile.ForAllMaps(IgnoreUnmappedProperties);
        }

        public static void IgnoreUnmapped(this IProfileExpression profile, Func<TypeMap, bool> filter)
        {
            profile.ForAllMaps((map, expr) =>
            {
                if (filter(map))
                {
                    IgnoreUnmappedProperties(map, expr);
                }
            });
        }

        public static void IgnoreUnmapped(this IProfileExpression profile, Type src, Type dest)
        {
            profile.IgnoreUnmapped((TypeMap map) => map.SourceType == src && map.DestinationType == dest);
        }

        [ObsoleteAttribute("Does not work. Use method that ignores all unmapped members instead.")]
        public static void IgnoreUnmapped<TSrc, TDest>(this IProfileExpression profile)
        {
            profile.IgnoreUnmapped(typeof(TSrc), typeof(TDest));
        }

        /* USAGE: https://stackoverflow.com/questions/954480/automapper-ignore-the-rest/31182390#31182390
          
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Src, Dest>();
                cfg.IgnoreUnmapped();        // Ignores unmapped properties on all maps
                cfg.IgnoreUnmapped<Src, Dest>();  // Ignores unmapped properties on specific map
            });

            // or add  inside a profile
            public class MyProfile : Profile
            {
               this.IgnoreUnmapped();
               CreateMap<MyType1, MyType2>();
            }
        */
    }
}
