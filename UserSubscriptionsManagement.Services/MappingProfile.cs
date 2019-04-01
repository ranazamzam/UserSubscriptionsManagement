using AutoMapper;
using AutoMapper.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserSubscriptionsManagement.Contracts.DataContracts;
using UserSubscriptionsManagement.Domain.Models;

namespace UserSubscriptionsManagement.Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add Maps Here
            CreateMap<User, UserData>().ReverseMap();
            CreateMap<Subscription, SubscriptionData>().ReverseMap();
        }

    }

    /// <summary>
    /// Mapping Extentions to Convert a Model To Entity and vice versa.
    /// </summary>
    public static class MappingExtentions
    {
        public static User ToEntity(this UserData model)
        {
            return Mapper.Map<User>(model);
        }

        public static UserData ToModel(this User entity)
        {
            return Mapper.Map<UserData>(entity);
        }

        public static Subscription ToEntity(this SubscriptionData model)
        {
            return Mapper.Map<Subscription>(model);
        }

        public static SubscriptionData ToModel(this Subscription entity)
        {
            return Mapper.Map<SubscriptionData>(entity);
        }

        public static List<UserData> ToModel(this List<User> entity)
        {
            return Mapper.Map<List<UserData>>(entity);
        }

        public static List<SubscriptionData> ToModel(this List<Subscription> entity)
        {
            return Mapper.Map<List<SubscriptionData>>(entity);
        }
    }
}
