using System;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http.Authentication;

using Newtonsoft.Json;

using AutoMapper;

using NTWebApp.DBAccess;
using NTWebApp.Models;
using NTWebApp.Models.Home;
using NTWebApp.Models.Account;
using NTWebApp.Models.InvoiceManager;
using NTWebApp.UIClasses;
using NTWebApp.Controllers;

namespace NTWebApp
{
    public static class Global
    { }

    public static class DBMapperFactory
    {
        public static Dictionary<string, string> DBMapper;

        public static void InitializeDBMapperFactory()
        {
            DBMapper = new Dictionary<string, string>();
            DBMapper.Add("CA001_IM", "server=ttechprod.cw8rzstofewz.us-west-2.rds.amazonaws.com;userid=noah089736;pwd=089736noahTYJ;port=3306;database=CA001_IM;sslmode=none;");
        }
    }

    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            object o;
            tempData.TryGetValue(key, out o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }

    public static class SessionExtensions
    {
        public static void Put<T>(this ISession sessionData, string key, T value) where T : class
        {
            sessionData.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession sessionData, string key) where T : class
        {
            string s;
            s = sessionData.GetString(key);
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(s);
            }
        }
    }

    public static class WeekGenerator
    {
        public static IEnumerable<Dictionary<string, object>> GetAllWeeksForYear(int year)
        {
            var jan1 = new DateTime(year, 1, 1);
            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            var weeks =
                Enumerable
                    .Range(0, 54)
                    .Select(i => new {
                        weekStart = startOfFirstWeek.AddDays(i * 7)
                    })
                    .TakeWhile(x => x.weekStart.Year <= jan1.Year)
                    .Select(x => new {
                        x.weekStart,
                        weekFinish = x.weekStart.AddDays(6)
                    })
                    .SkipWhile(x => x.weekFinish < jan1.AddDays(1))
                    .Select((x, i) => new Dictionary<string, object>()
                    {
                        { "WeekStart", x.weekStart},
                        { "WeekEnd", x.weekFinish},
                        { "WeekNum", i + 1}
                    });

            return weeks;
        }
    }

    public static class MonthGenerator
    {
        public static IEnumerable<Dictionary<string, object>> GetAllMonthsForYear(int year)
        {
            var jan1 = new DateTime(year, 1, 1);
            var startOfFirstMonth = jan1;
            var months =
                Enumerable
                    .Range(0, 11)
                    .Select(i => new {
                        monthStart = startOfFirstMonth.AddMonths(i)
                    })
                    .TakeWhile(x => x.monthStart.Year <= jan1.Year)
                    .Select(x => new {
                        x.monthStart,
                        monthFinish = x.monthStart.AddMonths(1).AddDays(-1)
                    })
                    .SkipWhile(x => x.monthFinish < jan1.AddDays(1))
                    .Select((x, i) => new Dictionary<string, object>()
                    {
                        { "MonthStart", x.monthStart},
                        { "MonthEnd", x.monthFinish},
                        { "MonthNum", i + 1}
                    });

            return months;
        }
    }

    public static class YearGenerator
    {
        public static IEnumerable<int> GetYears(int numberOfYears)
        {
            var thisYear = DateTime.Today.Year;
            var years =
                Enumerable
                    .Range(0, numberOfYears)
                    .Select(i => thisYear - i);

            return years;
        }
    }

    public static class AutoMapperFactory
    {
        public static MapperConfiguration AccountViewModel_UIUserMapping;
        public static MapperConfiguration UserDetailViewModel_UIUserMapping;
        public static MapperConfiguration ExpenseCategViewModel_UIExpenseCateg;
        public static MapperConfiguration ExpenseViewModel_UIExpense;
        public static MapperConfiguration MessageBoardMsgViewModel_UIMessageBoardMsg;
        public static MapperConfiguration MonthViewItemViewModel_UIMonthExpense;
        public static MapperConfiguration WeekViewItemViewModel_UIWeekExpense;

        public static void InitializeAutoMapperFactory()
        {
            try
            {
                AccountViewModel_UIUserMapping = new MapperConfiguration(cfg => cfg.CreateMap<AccountViewModel, UIUser>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                    .ForMember(dest => dest.SecurityToken, opt => opt.MapFrom(src => src.SecurityToken))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.MapFrom(src => src.ProfilePhotoUrl))
                    .ForMember(dest => dest.DBInstance, opt => opt.MapFrom(src => src.DBInstance))
                    .ForMember(dest => dest.RetryPassword, opt => opt.MapFrom(src => src.RetryPassword))
                    .ReverseMap());

                UserDetailViewModel_UIUserMapping = new MapperConfiguration(cfg => cfg.CreateMap<UserDetailViewModel, UIUser>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                    .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ForMember(dest => dest.Password, opt => opt.Ignore())
                    .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                    .ForMember(dest => dest.SecurityToken, opt => opt.Ignore())
                    .ForMember(dest => dest.ProfilePhotoUrl, opt => opt.Ignore())
                    .ForMember(dest => dest.DBInstance, opt => opt.Ignore())
                    .ForMember(dest => dest.RetryPassword, opt => opt.Ignore())
                    .ReverseMap());

                ExpenseCategViewModel_UIExpenseCateg = new MapperConfiguration(cfg => cfg.CreateMap<ExpenseCategViewModel, UIExpenseCateg>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.ExpenseCategId, opt => opt.MapFrom(src => src.ExpenseCategId))
                    .ForMember(dest => dest.ExpenseCategName, opt => opt.MapFrom(src => src.ExpenseCategName))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.IsDefault, opt => opt.MapFrom(src => src.IsDefault))
                    .ForMember(dest => dest.OrderVal, opt => opt.MapFrom(src => src.OrderVal))
                    .ReverseMap());

                ExpenseViewModel_UIExpense = new MapperConfiguration(cfg => cfg.CreateMap<ExpenseViewModel, UIExpense>()
                    .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.ExpenseId))
                    .ForMember(dest => dest.ExpenseCategId, opt => opt.MapFrom(src => src.ExpenseCategId))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
                    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                    .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                    .ReverseMap());

                MessageBoardMsgViewModel_UIMessageBoardMsg = new MapperConfiguration(cfg => cfg.CreateMap<MessageBoardMsgViewModel, UIMessageBoardMsg>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message))
                    .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Time))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.MessageId, opt => opt.Ignore())
                    .ReverseMap());

                MonthViewItemViewModel_UIMonthExpense = new MapperConfiguration(cfg => cfg.CreateMap<MonthViewItemViewModel, UIMonthExpense>()
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                    .ForMember(dest => dest.ExpenseCategId, opt => opt.MapFrom(src => src.ExpenseCategId))
                    .ForMember(dest => dest.ExpenseCategName, opt => opt.MapFrom(src => src.ExpenseCategName))
                    .ForMember(dest => dest.OrderVal, opt => opt.MapFrom(src => src.OrderVal))
                    .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
                    .ReverseMap());

                WeekViewItemViewModel_UIWeekExpense = new MapperConfiguration(cfg => cfg.CreateMap<WeekViewItemViewModel, UIWeekExpense>()
                     .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                     .ForMember(dest => dest.ExpenseCategId, opt => opt.MapFrom(src => src.ExpenseCategId))
                     .ForMember(dest => dest.ExpenseCategName, opt => opt.MapFrom(src => src.ExpenseCategName))
                     .ForMember(dest => dest.OrderVal, opt => opt.MapFrom(src => src.OrderVal))
                     .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
                     .ReverseMap());

                // Validate all mappings
                AccountViewModel_UIUserMapping.AssertConfigurationIsValid();
                UserDetailViewModel_UIUserMapping.AssertConfigurationIsValid();
                ExpenseCategViewModel_UIExpenseCateg.AssertConfigurationIsValid();
                ExpenseViewModel_UIExpense.AssertConfigurationIsValid();
                MessageBoardMsgViewModel_UIMessageBoardMsg.AssertConfigurationIsValid();
                MonthViewItemViewModel_UIMonthExpense.AssertConfigurationIsValid();
                WeekViewItemViewModel_UIWeekExpense.AssertConfigurationIsValid();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
