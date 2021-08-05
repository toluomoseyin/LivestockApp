using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using AnimalFarmsMarket.Data.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AnimalFarmsMarket.Data
{
    public static class Preseeder
    {
        private static string path = Path.GetFullPath(@"../AnimalFarmsMarket.Data/Data.Json/");

        private const string adminPassword = "Secret@123";
        private const string regularPassword = "P@ssw0rd";

        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            //Get the Db context
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (context.Users.Any())
            {
                return;
            }
            else
            {
                //Get Usermanager and rolemanager from IoC container
                var userManager = app.ApplicationServices.CreateScope()
                                              .ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var roleManager = app.ApplicationServices.CreateScope()
                                                .ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //Create role if it doesn't exists
                var roles = new string[] { "Admin", "Customer", "Agent", "Delivery" };
                foreach (var role in roles)
                {
                    var roleExist = await roleManager.RoleExistsAsync(role);
                    if (!roleExist)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                //Seed Category data
                var categoryList = GetSampleData<Category>(File.ReadAllText(path + "Category.json"));
                await context.Categories.AddRangeAsync(categoryList);

                //Seed Users with 1(one) Admin User
                var appUsers = GetSampleData<AppUser>(File.ReadAllText(path + "User.json"));
                var (adminCount, customerCount, agentCount, deliveryPersonCount) = (0, 0, 0, 0);

                foreach (var user in appUsers)
                {
                    if (adminCount < 1)
                    {
                        await userManager.CreateAsync(user, adminPassword);
                        await userManager.AddToRoleAsync(user, roles[0]);
                        ++adminCount;
                    }
                    else
                    {
                        await userManager.CreateAsync(user, regularPassword);
                        await userManager.AddToRoleAsync(user, roles[1]);
                        ++agentCount;
                    }

                    ConfirmUserEmail(user, userManager);
                }

                //Seed Livestocks
                var livestockList = GetSampleData<Livestock>(File.ReadAllText(path + "Livestock.json"));

                //Seed Agent data
                var agentList = GetSampleData<Agent>(File.ReadAllText(path + "Agent.json"));

                int iCount;
                var temp = 0;
                foreach (var user in agentList)
                {
                    await userManager.CreateAsync(user.AppUser, regularPassword);
                    var userResult = await userManager.FindByEmailAsync(user.AppUser.Email);
                    user.AppUserId = userResult.Id;
                    user.AppUser = null;
                    ConfirmUserEmail(userResult, userManager);

                    await userManager.AddToRoleAsync(userResult, roles[2]);

                    if (agentList.Count > 0)
                    {
                        for (iCount = temp; iCount < livestockList.Count; iCount++)
                        {
                            livestockList[iCount].AgentId = user.AppUserId;
                            user.Livestocks.Add(livestockList[iCount]);
                            if (iCount % 5 == 0 && iCount != 0)
                            {
                                temp = iCount + 1;
                                break;
                            }
                        }
                    }
                }
                await context.Agents.AddRangeAsync(agentList);

                //Seed delivery person data
                var deliveryPersons = GetSampleData<DeliveryPerson>(File.ReadAllText(path + "DeliveryPerson.json"));
                foreach (var item in deliveryPersons)
                {
                    await userManager.CreateAsync(item.AppUser, regularPassword);
                    var user = await userManager.FindByEmailAsync(item.AppUser.Email);
                    item.AppUserId = user.Id;
                    item.AppUser = null;

                    await userManager.AddToRoleAsync(user, roles[3]);
                }
                await context.DeliveryPersons.AddRangeAsync(deliveryPersons);

                //seed Broadcast data
                var broadcastList = GetSampleData<BroadcastNews>(File.ReadAllText(path + "Broadcast.json"));
                await context.BroadCastNews.AddRangeAsync(broadcastList);

                //Seed delivery mode data
                var deliveryModes = GetSampleData<DeliveryMode>(File.ReadAllText(path + "DeliveryMode.json"));
                await context.DeliveryModes.AddRangeAsync(deliveryModes);

                //Seed payment method
                var paymentMethods = GetSampleData<PaymentMethod>(File.ReadAllText(path + "PaymentMethod.json"));
                await context.PaymentMethods.AddRangeAsync(paymentMethods);

                //Seed shipping plan
                var shippingPlans = GetSampleData<ShippingPlan>(File.ReadAllText(path + "ShippingPlan.json"));
                await context.ShippingPlans.AddRangeAsync(shippingPlans);

                //Seed Animal Market
                var animalMarket = GetSampleData<Market>(File.ReadAllText(path + "AnimalMarket.json"));
                await context.Markets.AddRangeAsync(animalMarket);

                //Seed partner data
                var partnerList = GetSampleData<Partner>(File.ReadAllText(path + "Partner.json"));
                await context.Partners.AddRangeAsync(partnerList);

                await context.SaveChangesAsync();
            }
        }

        //Get sample data from json files
        private static List<T> GetSampleData<T>(string location)
        {
            var output = JsonSerializer.Deserialize<List<T>>(location, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }

        private static async void ConfirmUserEmail(AppUser user, UserManager<AppUser> userManager)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, token);
        }
    }
}
