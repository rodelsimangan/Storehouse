namespace StorehouseAdmin.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;
    using StorehouseAdmin.Models;
    using Storehouse.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<StorehouseAdmin.Models.StorehouseDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(StorehouseAdmin.Models.StorehouseDBContext context)
        {
            

            context.Roles.AddOrUpdate(r => r.Name,
                new ApplicationRole { Name = "Admin" },
                new ApplicationRole { Name = "Tenant" });

            //context.u
            if (!context.Users.Any(u => u.UserName == "admin"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);

                var adminuser = new ApplicationUser { UserName = "admin" };
                //user.Roles.Add("Admin");
                var userResult = manager.Create(adminuser, "pass01");
                if (userResult.Succeeded)
                {
                    manager.AddToRole(adminuser.Id, "Admin");
                }

                var tenantrrc = new ApplicationUser { UserName = "rrcuser" };
                //user.Roles.Add("Admin");
                var rrcResult = manager.Create(tenantrrc, "pass01");
                if (rrcResult.Succeeded)
                {
                    manager.AddToRole(tenantrrc.Id, "Tenant");
                }


                
                context.Templates.AddOrUpdate(t => t.Name,
                    new Templates { TenantId= tenantrrc.Id, Name = "Partners", ControllerName = "Partners", Sequence = 1, Type = "Parent", IncludeInMenu = true, RSSFeedFileName = "Partners" },
                    new Templates { TenantId = tenantrrc.Id, Name = "Events and Promos", ControllerName = "EventsAndPromos", Sequence = 2, Type = "Parent", IncludeInMenu = true, RSSFeedFileName = "Events and Promos" },
                    new Templates { TenantId = tenantrrc.Id, Name = "FAQ", ControllerName = "FAQ", Sequence = 3, Type = "Collapsables", IncludeInMenu = true, RSSFeedFileName = "FAQ" },
                    new Templates { TenantId = tenantrrc.Id, Name = "Robinsons Stores", ControllerName = "RobinsonsStore", Sequence = 4, Type = "RobinsonsStores", IncludeInMenu = true, RSSFeedFileName = "Robinsons Stores" }
                    //,new Templates { Name = "Robinsons Rewards Contact Info", ControllerName = "RewardsContactInfo", Sequence = 5, Type = "Parent", IncludeInMenu = true }
                    );

                context.Feedbacks.AddOrUpdate(y => y.From,
                    new Feedbacks { TenantId = tenantrrc.Id, IsMember = true, MemberId = "000 00 0001", From = "msoriquez@asiaweb.com.ph", Name = "Franco", ContactNumber = "09171129578", Subject = "Test Subject", Message = "Test Message", IsRead = false },
                    new Feedbacks { TenantId = tenantrrc.Id, IsMember = false, MemberId = null, From = "msoriquez@asiaweb.com.ph 2", Name = "Marco", ContactNumber = "09161219551", Subject = "Test Subject 2", Message = "Test Message 2", IsRead = false }
                    );
                
                var socialLookups = new List<Lookups>{
                    new Lookups {TenantId= tenantrrc.Id, Category = "SocialMedia", Name = "facebook", IsDeleted = false },
                    new Lookups {TenantId= tenantrrc.Id, Category = "SocialMedia", Name = "twitter", IsDeleted = false },
                    new Lookups {TenantId= tenantrrc.Id, Category = "SocialMedia", Name = "instagram", IsDeleted = false }
                };
                socialLookups.ForEach(s => context.Lookups.AddOrUpdate(l => l.Name, s));
                
                var terms = new List<TermsofUse>{
                    new TermsofUse {TenantId= tenantrrc.Id, Title = "Title Markup", Subtitle = "Subtitle Markup", Markup = "" },
                };
                terms.ForEach(s => context.TermsofUse.AddOrUpdate(l => l.Id, s));

                var sliders = new List<Sliders>{
                    new Sliders {TenantId= tenantrrc.Id, Sequence = 1, Title = "Playmat Carnival", SlideImage= "/Uploads/Sliders/Playmat Carnival CRM.png"},
                    new Sliders {TenantId= tenantrrc.Id, Sequence = 2, Title = "800x280", SlideImage= "/Uploads/Sliders/800x280.jpg"},
                    new Sliders {TenantId= tenantrrc.Id, Sequence = 3, Title = "800x280 2", SlideImage= "/Uploads/Sliders/800x280 2.jpg"},
                    new Sliders {TenantId= tenantrrc.Id, Sequence = 4, Title = "800x280 3", SlideImage= "/Uploads/Sliders/800x280 3.jpg"}
                };
                sliders.ForEach(s => context.Sliders.AddOrUpdate(l => l.Id, s));

                var contentsliders = new List<ContentSliders>{
                    new ContentSliders {TenantId= tenantrrc.Id, Sequence = 1, LeftRight = "Left Slider", SlideImage= "/Uploads/ContentSliders/500x300.jpg"},
                    new ContentSliders {TenantId= tenantrrc.Id, Sequence = 2, LeftRight = "Left Slider", SlideImage= "/Uploads/ContentSliders/500x300 2.jpg"},
                    new ContentSliders {TenantId= tenantrrc.Id, Sequence = 3, LeftRight = "Right Slider", SlideImage= "/Uploads/ContentSliders/500x300 3.jpg"},
                    new ContentSliders {TenantId= tenantrrc.Id, Sequence = 4, LeftRight = "Right Slider", SlideImage= "/Uploads/ContentSliders/500x300 4.jpg"}
                };
                contentsliders.ForEach(s => context.ContentSliders.AddOrUpdate(l => l.Id, s));

                var contents = new List<Contents>{
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Partners 1", Description= "Sample Description for Partners 1", Markup= "Sample Markup for Partners 1", Image="/Uploads/Contents/500x300.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Partners Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Partners 2", Description= "Sample Description for Partners 2", Markup= "Sample Markup for Partners 2", Image="/Uploads/Contents/500x300 2.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Partners Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Partners 3", Description= "Sample Description for Partners 3", Markup= "Sample Markup for Partners 3", Image="/Uploads/Contents/500x300 3.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Partners Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Events and Promos", Description= "Sample Description for Events and Promos", Markup= "Sample Markup for Events and Promos", Image="/Uploads/Contents/500x300 4.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Events and Promos Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Supermarket", Description= "Sample Description for Supermarket", Markup= "Sample Markup for Supermarket", Image="/Uploads/Contents/Supermarket.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Department Store", Description= "Sample Description for Department Store", Markup= "Sample Markup for Department Store", Image="/Uploads/Contents/Department Store.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Appliances", Description= "Sample Description for Appliances", Markup= "Sample Markup for Appliances", Image="/Uploads/Contents/Appliances.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Handyman", Description= "Sample Description for Handyman", Markup= "Sample Markup for Handyman", Image="/Uploads/Contents/Handyman.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Toys R Us", Description= "Sample Description for Toys R Us", Markup= "Sample Markup for Toys R Us", Image="/Uploads/Contents/Toysrus.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Daiso Japan", Description= "Sample Description for Daiso Japan", Markup= "Sample Markup for Daiso Japan", Image="/Uploads/Contents/Daiso.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Topshop", Description= "Sample Description for Topshop", Markup= "Sample Markup for Topshop", Image="/Uploads/Contents/Topshop.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"},
                    new Contents {TenantId= tenantrrc.Id, DateCreated = DateTime.Now, Name = "Topman", Description= "Sample Description for Topman", Markup= "Sample Markup for Topman", Image="/Uploads/Contents/Topman.jpg", IsPublished= true, IncludeInHomePage= true, PushToNewsFeed= false, TempName="Robinsons Stores Feed.xml"}
                };
                contents.ForEach(s => context.Contents.AddOrUpdate(l => l.Id, s));

                context.HomePageSettings.AddOrUpdate(h => h.Id, new HomePageSettings {
                    TenantId = tenantrrc.Id,
                    HeaderLogo = "/Uploads/Homepage/headerlogo.png",
                    FooterLogo = "/Uploads/Homepage/footerlogo.jpg",
                    CompanyName = "Robinsons Land Corporation Commercial Centers Division Level 2",
                    CompanyPhone = "+63.2.397.1888",
                    CompanyEmail = "customer.care@robinsonsland.com",
                    CompanyAddress = "Galleria Corporate Center EDSA corner Ortigas Avenue 1100 Quezon City",
                    SliderSpeed = 500,
                    AboutCompany = "Write something about the Company.",
                    EventsandPromos = "Write something about Events and Promos",
                });

                var socialMedialLinks = new List<SocialMediaLinks>{                    
                    new SocialMediaLinks {TenantId= tenantrrc.Id, Name = "facebook", Value= "http://www.facebook.com/robinsonsmyreward", IsDeleted=false },
                    new SocialMediaLinks {TenantId= tenantrrc.Id, Name = "twitter", Value= "http://www.twitter.com//robinsonsmyreward", IsDeleted=false },
                    new SocialMediaLinks {TenantId= tenantrrc.Id, Name = "instagram", Value= "http://www.instagram.com/robinsonsmyreward", IsDeleted=false },
                };
                socialMedialLinks.ForEach(m => context.SocialMediaLinks.AddOrUpdate(i => i.Name, m));
                
            }
        }
    }
}
