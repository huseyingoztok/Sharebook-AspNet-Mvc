namespace ShareBook.DAL.Migrations
{
    using ShareBook.Entities;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ShareBook.DAL.EntityFramework.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ShareBook.DAL.EntityFramework.DatabaseContext context)
        {
            ShareBookUser admin = new ShareBookUser()
            {
                Name = "Hüseyin",
                Surname = "GÖZTOK",
                Email = "huseyin.goztok@gmail.com",
                Username = "huseyin.goztok",
                isActive = true,
                activatedGuid = Guid.NewGuid(),
                isAdmin = true,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedBy = "huseyin.goztok",
                Password = "123",
                isDeleted = false,
                ProfilPhoto = "/Images/DefaultProfilImages/default1.png",


            };


            context.ShareBookUser.Add(admin);
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
            {

                int random = rnd.Next(1, 7);
                ShareBookUser user = new ShareBookUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    Username = $"user{i}",
                    isActive = true,
                    activatedGuid = Guid.NewGuid(),
                    isAdmin = false,
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = $"user{i}",
                    Password = "123",
                    isDeleted = false,
                    ProfilPhoto = "/Images/DefaultProfilImages/default" + random + ".png",


                };
                context.ShareBookUser.Add(user);
            }


            context.SaveChanges();
            List<ShareBookUser> shrUsers = context.ShareBookUser.ToList();

            //Fake Category
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Icon = FakeData.NameData.GetFirstName(),
                    Title = FakeData.PlaceData.GetStreetName(),
                    isActive = true,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedBy = "Hüseyin GÖZTOK",
                    Description = FakeData.NameData.GetSurname(),
                    isDeleted = false,


                };
                context.Category.Add(category);
                //Fake Shares
                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 10); j++)
                {
                    Sharing shares = new Sharing()
                    {
                        //Category = category, Kategorilerni paylaşımlarına eklediğimiz için bu tür bir koda gerek yok.
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Title = FakeData.TextData.GetAlphabetical(25),
                        ShareContent = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(3, 5)),
                        isDraft = false,
                        Owner = shrUsers[FakeData.NumberData.GetNumber(0, shrUsers.Count - 1)],
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = DateTime.Now,
                        ModifiedBy = shrUsers[FakeData.NumberData.GetNumber(0, shrUsers.Count - 1)].Username,
                        isDelete = false,
                        ImageUrl = "/Images/SharingImage/sitelogo.png",


                    };
                    category.Shares.Add(shares);

                    for (int k = 0; k < FakeData.NumberData.GetNumber(1, 5); k++)
                    {
                        Comment cmt = new Comment()
                        {
                            //Sharing=shares, Paylaşımların commentlerine eklediğimiz için bu tür bir koda gerek yok.
                            commentText = FakeData.TextData.GetSentence(),
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = DateTime.Now,
                            ModifiedBy = shrUsers[FakeData.NumberData.GetNumber(0, shrUsers.Count - 1)].Username,
                            Owner = shrUsers[FakeData.NumberData.GetNumber(0, shrUsers.Count - 1)],

                            isDelete = false,
                        };
                        shares.Comments.Add(cmt);

                    }

                    for (int m = 0; m < shares.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            likedUser = shrUsers[m],

                        };
                        shares.Likes.Add(liked);
                    }

                }
            }
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
