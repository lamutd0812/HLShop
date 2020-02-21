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
	}
}