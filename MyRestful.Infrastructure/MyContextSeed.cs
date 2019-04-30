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
                                Abbreviation = "中国"
                            },
                            new Country
                            {
                                EnglishName = "USA",
                                ChineseName = "美利坚合众国",
                                Abbreviation = "美国"
                            },
                            new Country
                            {
                                EnglishName = "Finland",
                                ChineseName = "芬兰",
                                Abbreviation = "芬兰"
                            },
                            new Country
                            {
                                EnglishName = "UK",
                                ChineseName = "大不列颠及爱尔兰联合王国",
                                Abbreviation = "英国"
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
