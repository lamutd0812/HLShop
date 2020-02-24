using HLShop.Model.Models;
using System.Collections.Generic;

namespace HLShop.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HLShop.Data.HLShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HLShop.Data.HLShopDbContext context)
        {
            //CreateUser(context);
            CreateProductCategorySample(context);
            CreateSlide(context);
            CreatePage(context);
            CreateContextDetail(context);

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }

        private void CreateUser(HLShopDbContext context)
        {
            //var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new HLShopDbContext()));

            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new HLShopDbContext()));

            //var user = new ApplicationUser()
            //{
            //    UserName = "huulam",
            //    Email = "lamutd0812@gmail.com",
            //    EmailConfirmed = true,
            //    BirthDay = DateTime.Now,
            //    Fullname = "Nguyen Huu Lam"
            //};

            //manager.Create(user, "1234564$");
            //if (!roleManager.Roles.Any())
            //{
            //    roleManager.Create(new IdentityRole { Name = "Admin" });
            //    roleManager.Create(new IdentityRole { Name = "User" });
            //}

            //var adminUser = manager.FindByEmail("lamutd0812@gmail.com");

            //manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        }

        private void CreateProductCategorySample(HLShop.Data.HLShopDbContext context)
        {
            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory(){Name = "Điện lạnh", Alias = "dien-lanh", Status = true},
                    new ProductCategory(){Name = "Viễn thông", Alias = "vien-thong", Status = true},
                    new ProductCategory(){Name = "Đồ gia dụng", Alias = "do-gia-dung", Status = true},
                    new ProductCategory(){Name = "Đồ thể thao", Alias = "do-the-thao", Status = true},
                    new ProductCategory(){Name = "Điện thoại", Alias = "dien-thoai", Status = true}
                };
                context.ProductCategories.AddRange(listProductCategory);
                context.SaveChanges();
            }
        }

        //private void CreateFooter(HLShopDbContext context)
        //{
        //    if (context.Footers.Count(x => x.ID == CommonConstants.DefaultFooterId) == 0)
        //    {
        //        string content = "";
        //    }
        //}

        private void CreateSlide(HLShopDbContext context)
        {
            if (context.Slides.Count() == 0)
            {
                List<Slide> listSlide = new List<Slide>()
                {
                    new Slide(){Name = "Slide 1",
                        DisplayOrder = 1
                        , Status = true,
                        Url = "#",
                        Image = "/Assets/client/images/bag.jpg",
                        Content = @"<h2>FLAT 50% 0FF</h2>
										<label>FOR ALL PURCHASE <b>VALUE</b></label>
										<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit,
											 sed do eiusmod tempor incididunt ut labore et </ p >
										<span class=""on-get"">GET NOW</span>"},
                    new Slide()
                    {
                        Name = "Slide 2",
                        DisplayOrder = 2,
                        Status = true, Url = "#",
                        Image = "/Assets/client/images/bag1.jpg",
                        Content = @"<h2>FLAT 50% 0FF</h2>
										<label>FOR ALL PURCHASE <b>VALUE</b></label>
										<p>Lorem ipsum dolor sit amet, consectetur adipisicing elit,
											sed do eiusmod tempor incididunt ut labore et </p>
										<span class=""on-get"">GET NOW</span>"
                    }
                };
                context.Slides.AddRange(listSlide);
                context.SaveChanges();
            }
        }

        private void CreatePage(HLShopDbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                var page = new Page()
                {
                    Name = "Introduction",
                    Alias = "gioi-thieu",
                    Content = "There was an advert in the 90’s which said “it does exactly what it says on the tin” and with Classicfootballshirts you pretty much get the same! Founded by two students at Uni in Manchester the company has grown to become one of the world’s leading suppliers of retro football shirts. Starting by simply selling their own collection on eBay, the students found there was a huge demand for retro football shirts and began sourcing shirts from around the globe. In 10 years Classic Football Co. has grown to a company with a £multi-million turnover employing 20 members of staff." +
                              "At the start of each season, the kitman at a football club will order a certain number of kit and if they don’t use them all then what happens to it? Well Classicfootballshirts buys all the stock they have left and puts it on their site. It’s not just football shirts, when I went around the warehouse there were football boots, shorts, even AC Milan underpants!" +
                              "With football clubs changing kits so frequently grrrrrr, retailers and manufacturers can get left with stock left over. The Classic Football Co. takes this stock and sells it on. Many of the items are brand new still in their original packaging!",
                    Status = true

                };
                context.Pages.Add(page);
                context.SaveChanges();
            }
        }

        private void CreateContextDetail(HLShopDbContext context)
        {
            if (context.ContactDetails.Count() == 0)
            {
                var contactDetail = new ContactDetail()
                {
                    Name = "HULA Sports",
                    Phone = "0984545747",
                    Email = "lamutd0812@gmail.com",
                    Website = "http://hulasports.vn",
                    Address = "Thanh Binh service complex, Thanh Binh, Chuong My, Hanoi",
                    Other = "",
                    Lat = 20.9138613,
                    Lng = 105.6266398,
                    Status = true
                };
                context.ContactDetails.Add(contactDetail);
                context.SaveChanges();
            }
        }
    }
}