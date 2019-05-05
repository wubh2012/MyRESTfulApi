using Microsoft.Extensions.Logging;
using MyRestful.Core;
using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Infrastructure
{
    public class MyContextSeed
    {
        public static async Task SeedAsync(MyContext myContext, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                if (!myContext.Countries.Any())
                {
                    myContext.Countries.AddRange(
                        new List<Country>
                        {
                            new Country
                            {
                                EnglishName = "China",
                                ChineseName = "中华人民共和国",
                                Abbreviation = "中国",
                                Cities = new List<City>
                                {
                                    new City { Name = "北京" },
                                    new City { Name = "上海" },
                                    new City { Name = "成都" },
                                    new City { Name = "广州" },
                                }
                            },
                            new Country
                            {
                                EnglishName = "USA",
                                ChineseName = "美利坚合众国",
                                Abbreviation = "美国",
                                Cities = new List<City>
                                {
                                    new City { Name = "洛杉矶" },
                                    new City { Name = "华盛顿" },
                                    new City { Name = "迈阿密" },
                                    new City { Name = "纽约" },
                                }
                            },
                            new Country
                            {
                                EnglishName = "Finland",
                                ChineseName = "芬兰",
                                Abbreviation = "芬兰",
                            },
                            new Country
                            {
                                EnglishName = "UK",
                                ChineseName = "大不列颠及爱尔兰联合王国",
                                Abbreviation = "英国",
                                Cities = new List<City>
                                {
                                    new City { Name = "伦敦" },
                                    new City { Name = "爱尔兰" },
                                   
                                }
                            }
                        }
                        );
                    await myContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var logger = loggerFactory.CreateLogger<MyContextSeed>();
                    logger.LogError(ex.Message);
                    await SeedAsync(myContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }
    }
}
