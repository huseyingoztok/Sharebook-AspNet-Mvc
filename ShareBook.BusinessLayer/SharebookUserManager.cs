using ShareBook.BusinessLayer.Abstract;
using ShareBook.BusinessLayer.Results;
using ShareBook.Common.Helpers;
using ShareBook.DAL.EntityFramework;
using ShareBook.Entities;
using ShareBook.Entities.EntityViewModels;
using ShareBook.Entities.Messages;
using ShareBook.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareBook.BusinessLayer
{
    public class SharebookUserManager : ManagerBase<ShareBookUser>,IDisposable
    {
        public BusinessLayerResult<ShareBookUser> RegisterUser(RegisterViewModel model)
        {
            ShareBookUser user = Find(x => x.Email == model.Email || x.Username == model.Username);
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();

            if (user != null&&!user.isDeleted)
            {
                    if (user.Username == model.Username)
                    {
                        //res.Errors.Add("Kullanıcı adı kayıtlı");
                        res.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                    }

                    if (user.Email == model.Email)
                    {
                        //res.Errors.Add("E-posta adresi kayıtlı");
                        res.AddError(ErrorMessagesCode.EmailAlreadyExist, "E-posta adresi kayıtlı");
                    }
                
                
            }
            else
            {
                Random rnd = new Random();
                int random=rnd.Next(1, 7);
                int result = Insert(new ShareBookUser()
                {
                    
                    Surname = model.Surname,
                    Name = model.Name,
                    activatedGuid = Guid.NewGuid(),
                    Email = model.Email,
                    Password = model.Password,
                    Username = model.Username,
                    ProfilPhoto = "/Images/DefaultProfilImages/default"+random+".png",
                    isActive = false,
                    isAdmin = false,
                    isDeleted = false,
                });

                if (result > 0)
                {
                    res.Result = Find(x => x.Username == model.Username && x.Email == model.Email);
                    string siteUri = ConfigHelper.getConfig<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/ActivateUser/{res.Result.activatedGuid}";
                    string body = $"Merhaba {res.Result.Username};<br/><br/>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>Tıklayınız</a>.";
                    MailHelper.SendMail(body, res.Result.Email, "Sharebook Hesap Aktivasyonu Hk.");
                }
            }
            return res;
        }
        public BusinessLayerResult<ShareBookUser> RemoveByUserID(int id)
        {
            ShareBookUser user = Find(x => x.Id == id);
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            if (user!=null)
            {
                res.Result = Find(x => x.Username == user.Username && x.Email == user.Email);
                res.Result.activatedGuid = Guid.NewGuid();
                Update(res.Result);
                string siteUri = ConfigHelper.getConfig<string>("SiteRootUri");
                string activateUri = $"{siteUri}/Home/DeleteUserAccount/{res.Result.activatedGuid}";
                string body = $"Merhaba {res.Result.Username};<br/><br/>Hesabınızı Kalıcı Olarak Silmek <a href='{activateUri}' target='_blank'>Tıklayınız</a>.";
                MailHelper.SendMail(body, res.Result.Email, "Sharebook Hesap Silme İşlemi Hk.");
            }
            else
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Kullanıcı Bulunamadı.");
            }

            return res;
        }


        public BusinessLayerResult<ShareBookUser> iForgotPassword(ShareBookUser model)
        {
            ShareBookUser user = Find(x => x.Id == model.Id);
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            if (user!=null)
            {
               
                res.Result = user;
                res.Result.activatedGuid = Guid.NewGuid();
                if (Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessagesCode.ProfileNotUpdated, "Profil Güncellenemedi...");
                }
                else
                {
                    res.Result = Find(x => x.Username == model.Username && x.Email == model.Email);
                    res.Result.activatedGuid = Guid.NewGuid();
                    Update(res.Result);
                    string siteUri = ConfigHelper.getConfig<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/ForgotPasswordActivateUser/{res.Result.activatedGuid}";
                    string body = $"Merhaba {res.Result.Username};<br/><br/>Hesabınızın şifresini değiştirmek için <a href='{activateUri}' target='_blank'>Tıklayınız</a>.";
                    MailHelper.SendMail(body, res.Result.Email, "Sharebook Şifremi Unuttum Hk.");
                }

              
            }
            else
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Böyle bir kullanıcı bulunamadı");
                
            }

            return res;
        }

        public BusinessLayerResult<ShareBookUser> LoginUser(LoginViewModel model)
        {
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            res.Result = Find(x => x.Username == model.Username && x.Password == model.Password && !x.isDeleted);



            if (res.Result != null)
            {
                if (!res.Result.isActive)
                {
                    //res.Errors.Add("Kullanıcı hesabı aktif edilmemiş lütfen E-mail hesabınızı kontrol ediniz.");
                    res.AddError(ErrorMessagesCode.UserIsNotActive, "Kullanıcı hesabı aktifleştirilmemiş.");
                    res.AddError(ErrorMessagesCode.ChecYourEmail, "Lütfen E-mail hesabınızı kontrol ediniz.");
                }

                //TODO: USER BAN !

            }
            else
            {
                //res.Errors.Add("Kullanıcı adı ya da şifre uyuşmuyor.");
                res.AddError(ErrorMessagesCode.UsernameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
            }

            return res;

        }

        public BusinessLayerResult<ShareBookUser> ActivateUser(Guid ActivateId)
        {
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            res.Result = Find(x => x.activatedGuid == ActivateId);

            if (res.Result != null)
            {
                if (res.Result.isActive&&!res.Result.isDeleted)
                {
                    res.AddError(ErrorMessagesCode.UserAlreadyActive, "Kullanıcı zaten aktif");
                    return res;
                }
                res.Result.isActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessagesCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<ShareBookUser> forgotActivateUser(Guid ActivateId)
        {
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            res.Result = Find(x => x.activatedGuid == ActivateId);

            if (res.Result==null)
            
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Böyle bir kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<ShareBookUser> getUserById(int id)
        {
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            res.Result = Find(x => x.Id == id);

          

            if (res.Result == null)
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<ShareBookUser> UpdateProfile(ShareBookUser model)
        {
            List<ShareBookUser> user = ListQueryable().Where(x => (x.Username == model.Username || x.Email == model.Email)&&x.Id!=model.Id).ToList();
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            if (user.Count > 0)
            {
                foreach (var item in user)
                {
                    if (item.Id != model.Id)
                    {
                        if (item.Username == model.Username)
                        {
                            res.AddError(ErrorMessagesCode.UsernameAlreadyExist, "Kullanıcı adı kayıtlı");
                        }

                        if (item.Email == model.Email)
                        {
                            res.AddError(ErrorMessagesCode.EmailAlreadyExist, "E-posta adresi kayıtlı");
                        }
                    }

                }
                return res;
            }
            res.Result = Find(x => x.Id == model.Id);
            res.Result.Email = model.Email;
            res.Result.Name = model.Name;
            res.Result.Username = model.Username;
            res.Result.Surname = model.Surname;
            res.Result.Password = model.Password;
            if (model.ProfilPhoto != null)
            {
                res.Result.ProfilPhoto = model.ProfilPhoto;
            }

            if (Update(res.Result) == 0)
            {
                res.AddError(ErrorMessagesCode.ProfileNotUpdated, "Profil Güncellenemedi...");
            }

            return res;
        }

        public BusinessLayerResult<ShareBookUser> RemoveUser(ShareBookUser user)
        {
            ShareBookUser deleteUser = Find(x => x.Id == user.Id);
            BusinessLayerResult<ShareBookUser> res = new BusinessLayerResult<ShareBookUser>();
            
            if (deleteUser!=null)
            {
                res.Result = deleteUser;
                res.Result.isDeleted = true;
                int deleteRes = Update(res.Result);

                if (deleteRes==0)
                {
                    res.AddError(ErrorMessagesCode.ProfileNotDeleted, "Profil Silinemedi");

                }

                return res;
            }
            else
            {
                res.AddError(ErrorMessagesCode.UserNotFound, "Kullanıcı  bulunamadı.");
                return res;
            }
           

        }

       
    }
}
