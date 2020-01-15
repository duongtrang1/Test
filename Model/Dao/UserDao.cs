﻿using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;


namespace Model.Dao
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }

        public long Insert(User entity) {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        //Hàm Update
        public bool Update(User entity)
        {
            try
            {
                //gán vào 1 đối tượng
                var user = db.Users.Find(entity.ID);
                //Update các trường
                user.Name = entity.Name;

                //Kiểm tra xem người dùng có muốn đổi mật khẩu không
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false ;
            }            
        }

        //Hàm trả về danh sách tất cả Users
        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize)

        {
            //Tìm kiếm User
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            //Sắp xếp theo thứ tự ngày tạo
            return model.OrderBy(x => x.CreateDate).ToPagedList(page, pageSize);               
        }

        public User GetById(string userName)
        {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }
        
        //Hàm xem chi tiết 
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);//Phương thức tìm kiếm theo khóa chính 
           
        }

        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status==false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }

        //Xóa
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
