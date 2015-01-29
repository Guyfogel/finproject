using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Globalization;
using System.Data;

namespace DAL
{
    public class CarRepository
    {
        public bool logValidation(string name, string password)
        {
            using (CarDBContext context = new CarDBContext())
            {
                try
                {
                    context.Users.First(u => u.Username == name && u.Password == password);
                    return true;
                }
                catch (ArgumentNullException)
                {
                    return false;
                }
                catch (InvalidOperationException)
                {
                    return false; 
                }
            }
        }

        public void AddUser(Users user) 
        {
            using (CarDBContext context = new CarDBContext())
            {
                //Users user = new Users() { Username = username, Password = password, Email = email, PersonNum = personnumber, Gender=(Genders)Enum.Parse(typeof(Genders), gender, true),Role=Roles.Customer };
                //if (pic!=null)
                //{
                //    user.Pic = pic;
                //}
                //Gender gen = new Gender
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public Users UpdateUser(int userID, string username, string password, string name, string email, string gender, string role, int personnumber, byte[] photo)
        {
            using (CarDBContext context = new CarDBContext())
            {
                Users UserToUpdate = GetUserByID(userID);
                if (!string.IsNullOrEmpty(username)) UserToUpdate.Username = username;
                if (!string.IsNullOrEmpty(password)) UserToUpdate.Password = password;
                if (!string.IsNullOrEmpty(name)) UserToUpdate.Name = name;
                if (!string.IsNullOrEmpty(email)) UserToUpdate.Email = email;
                if (!string.IsNullOrEmpty(gender)) UserToUpdate.Gender = (Genders)Enum.Parse(typeof(Genders), gender, true);
                if (!string.IsNullOrEmpty(role)) UserToUpdate.Role = (Roles)Enum.Parse(typeof(Roles), role, true);
                if (personnumber != 0) UserToUpdate.PersonNum = personnumber;
                if (photo != null) UserToUpdate.Pic = photo;
                context.Entry(UserToUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return UserToUpdate;
            }
        }

        public void RemoveUser(Users user)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.Users.Attach(user);
                context.Users.Remove(user);
                context.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public Users GetUser (string username)
        {
             using (CarDBContext context = new CarDBContext())
             {
                 var query = (from u in context.Users
                             where u.Username == username
                             select u).FirstOrDefault();

                 return (Users)query;             
             }
        }

        public Users GetUserByID(int userID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Users
                             where u.UserID == userID
                             select u).FirstOrDefault();

                return (Users)query;
            }
        }

        public IEnumerable<Users> GetAllUsers()
        {
            using (CarDBContext context = new CarDBContext())
            {
                return context.Users.ToArray();
            }
        }

        public IEnumerable<Orders> GetOrdersByFilter (string carmanufacturer, string carmodel, DateTime lenddate, DateTime returndate)
        {
            using (CarDBContext context = new CarDBContext())
             {
                var query =  from u in context.Orders
                             where (u.Car.CarType.Manufacturer == carmanufacturer)&&(u.Car.CarType.CarModel==carmodel)&&(u.LendDate==lenddate)&&(u.ReturnDate==returndate)
                             select u;

                return query.ToArray();
             }
        }
        public Orders GetOrderByFilter(string username, string manufacturer, string carmodel)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from o in context.Orders
                             join c in context.Cars on o.CarID equals c.CarID
                             join ct in context.CarTypes on c.CarTypeID equals ct.CarTypeID
                             join u in context.Users on o.UserID equals u.UserID
                             where (u.Username==username||ct.Manufacturer==manufacturer&&ct.CarModel==carmodel)
                             //where (u.CarModel == carmodel) && (u.Manufacturer == manufacturer)
                             select o).FirstOrDefault();

                return query;
            }
        }
        public IEnumerable<object> GetAllOrders()
        {
                using (CarDBContext context = new CarDBContext())
                {
                    var query = (from o in context.Orders
                                 join c in context.Cars on o.CarID equals c.CarID
                                 join ct in context.CarTypes on c.CarTypeID equals ct.CarTypeID
                                 join u in context.Users on o.UserID equals u.UserID
                                 //where (u.CarModel == carmodel) && (u.Manufacturer == manufacturer)
                                 select new { OrderID = o.OrderID, CarID = c.CarID, Manufacturer = ct.Manufacturer, CarModel = ct.CarModel, PricePerDay = ct.PricePerDay, PricePerLateDay = ct.PricePerLateDay, Username = u.Username, LendDate = o.LendDate, ReturnDate = o.ReturnDate, ActualReturnDate=o.ActualReturnDate});
                                 

                    return query.ToArray();
                }
        }

        public IEnumerable<object> GetAllOrdersByUsername(string username)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from o in context.Orders
                             join c in context.Cars on o.CarID equals c.CarID
                             join ct in context.CarTypes on c.CarTypeID equals ct.CarTypeID
                             join u in context.Users on o.UserID equals u.UserID
                             where u.Username==username
                             //where (u.CarModel == carmodel) && (u.Manufacturer == manufacturer)
                             select new { OrderID = o.OrderID, CarID = c.CarID, Manufacturer = ct.Manufacturer, CarModel = ct.CarModel, PricePerDay = ct.PricePerDay, PricePerLateDay = ct.PricePerLateDay, Username = u.Username, LendDate = o.LendDate, ReturnDate = o.ReturnDate, ActualReturnDate = o.ActualReturnDate });


                return query.ToArray();
            }
        }
   
        public object GetOrderForReceipt(int orderID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from o in context.Orders
                             join c in context.Cars on o.CarID equals c.CarID
                             join ct in context.CarTypes on c.CarTypeID equals ct.CarTypeID
                             join u in context.Users on o.UserID equals u.UserID
                             //where (u.CarModel == carmodel) && (u.Manufacturer == manufacturer)
                             select new { OrderID = o.OrderID, CarID = c.CarID,Manufacturer = ct.Manufacturer, CarModel = ct.CarModel, PricePerDay = ct.PricePerDay, PricePerLateDay = ct.PricePerLateDay, Username = u.Username, LendDate = o.LendDate, ReturnDate = o.ReturnDate, ActualReturnDate = o.ActualReturnDate }).FirstOrDefault();


                return query;
            }
        }

        public Orders GetOrderbyID(int orderID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Orders
                             where u.OrderID==orderID
                             select u).FirstOrDefault();
                return query;
            }
        }

        public bool isUserInOrder(int UserID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Orders
                             where u.UserID == UserID
                             select u).FirstOrDefault();
                return query!=null;  
            }
        }

        public bool isCarInOrder(int CarID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Orders
                             where u.CarID == CarID
                             select u).FirstOrDefault();
                return query != null;
            }
        }

        public void AddOrder(Orders order)
        {
            using (CarDBContext context = new CarDBContext())
            {
                //Users user = new Users() { Username = username, Password = password, Email = email, PersonNum = personnumber, Gender=(Genders)Enum.Parse(typeof(Genders), gender, true),Role=Roles.Customer };
                //if (pic!=null)
                //{
                //    user.Pic = pic;
                //}
                //Gender gen = new Gender
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        public Orders UpdateOrder(int OrderID, Cars car, Users user, string lenddate, string returndate)
        {
            using (CarDBContext context = new CarDBContext())
            {
                Orders OrderToUpdate = GetOrderbyID(OrderID);
                if (car != null)
                {
                    context.Cars.Attach(OrderToUpdate.Car = car);
                    OrderToUpdate.CarID = car.CarID;
                }
                if (user != null)
                {
                    context.Users.Attach(OrderToUpdate.User = user);
                    OrderToUpdate.UserID = user.UserID;
                }
                if (!string.IsNullOrEmpty(lenddate)){
                DateTime Ldate = DateTime.ParseExact(lenddate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    OrderToUpdate.LendDate=Ldate;
                }
                if (!string.IsNullOrEmpty(returndate))
                {
                DateTime Rdate = DateTime.ParseExact(returndate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    OrderToUpdate.ReturnDate=Rdate;
                }
                context.Entry(OrderToUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return OrderToUpdate;
            }
        }

        public void RemoveOrder(Orders order)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.Orders.Attach(order);
                context.Orders.Remove(order);
                context.Entry(order).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        //public Cars GetCar(string carmodel, string gear, string manufacturer, DateTime Startdate, DateTime Enddate)
        //{
        //    using (CarDBContext context = new CarDBContext())
        //    {
        //        var query = (from u in context.Orders
        //                     where (u.Car.CarType.Gear == gear)&&(u.Car.CarType.Manufacturer==manufacturer)&&(u.Car.CarType.CarModel==carmodel)&&(u.ActualReturnDate<Startdate)&&(u.LendDate>Enddate)
        //                     select u.Car).FirstOrDefault();
        //        return query;
        //    }
        //}

        public Cars GetCarByCartype(string manufacturer, string carmodel)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Cars
                             where (u.CarType.Manufacturer == manufacturer) && (u.CarType.CarModel == carmodel)
                             select u).FirstOrDefault();
                return query;
            }
        }

        public Cars GetCarByID(int carID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Cars
                             where u.CarID==carID
                             select u).FirstOrDefault();
                return query;
            }
        }

        public Locations GetLocation(string locationname)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Locations
                             where u.LocationName == locationname
                             select u).FirstOrDefault();
                return query;
            }
        }

        public void AddCarType(CarTypes cartype)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.CarTypes.Add(cartype);
                context.SaveChanges();
            }
        }

        public CarTypes UpdateCarType(int cartypeID, string manufacturer, string carmodel, string gear, decimal priceperday, decimal priceperlateday)
        {
            using (CarDBContext context = new CarDBContext())
            {
                CarTypes CarTypeToUpdate = GetCarTypeByID(cartypeID);
                if (manufacturer != null) CarTypeToUpdate.Manufacturer = manufacturer;
                if (carmodel != null) CarTypeToUpdate.CarModel = carmodel;
                if (gear != null) CarTypeToUpdate.Gear = gear;
                if (priceperday != 0) CarTypeToUpdate.PricePerDay = priceperday;
                if (priceperlateday != 0) CarTypeToUpdate.PricePerLateDay = priceperlateday;
                context.Entry(CarTypeToUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return CarTypeToUpdate;
            }
        }

        public void RemoveCarType(CarTypes cartype)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.CarTypes.Attach(cartype);
                context.CarTypes.Remove(cartype);
                context.Entry(cartype).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public bool isCarTypeInCar(int cartypeID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Cars
                             where u.CarTypeID == cartypeID
                             select u).FirstOrDefault();
                return query != null;
            }
        }

        public void AddCar(Cars car)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.Cars.Add(car);
                context.CarTypes.Attach(car.CarType);
                context.Locations.Attach(car.Location);
                context.SaveChanges();
            }
        }

        public Cars UpdateCar(CarTypes cartype, Locations location, int kilometrage, int CarID, byte[]photo, bool isAvailable)
        {
            using (CarDBContext context = new CarDBContext())
            {
                Cars CarToUpdate = GetCarByID(CarID);
                if (cartype != null)
                {
                    context.CarTypes.Attach(CarToUpdate.CarType = cartype);
                    CarToUpdate.CarTypeID = cartype.CarTypeID;
                }
                if (location != null)
                {
                    context.Locations.Attach(CarToUpdate.Location = location);
                    CarToUpdate.LocationID = location.LocationID;
                }
                if (kilometrage != 0) CarToUpdate.Kilometrage = kilometrage;
                if (photo != null) CarToUpdate.Photo = photo;
                if (CarToUpdate.isAvailable != isAvailable) CarToUpdate.isAvailable = isAvailable;
                context.Entry(CarToUpdate).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return CarToUpdate;
            }
        }

        public void RemoveCar(Cars car)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.Cars.Attach(car);
                context.Cars.Remove(car);
                context.Entry(car).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public decimal ReturnCar(Orders order, DateTime? actualreturndate)
        {
            using (CarDBContext context = new CarDBContext())
            {
                Cars car = GetCarByID(order.CarID);
                CarTypes cartype = GetCarTypeByID(car.CarTypeID);
                if (order != null)
                {
                    order.ActualReturnDate = actualreturndate;
                    context.Entry(order).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                }

                double originaldays = (order.ReturnDate - order.LendDate).TotalDays;
                double latedays = ((DateTime)order.ActualReturnDate - order.ReturnDate).TotalDays;
                if (latedays > 0)
                {
                    return (decimal)originaldays * cartype.PricePerDay + (decimal)latedays * cartype.PricePerLateDay;
                }
                else
                {
                    return (decimal)originaldays * cartype.PricePerDay;
                }


            }
        }

        public IEnumerable<object> GetAllCars()
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Cars
                             join v in context.CarTypes on u.CarTypeID equals v.CarTypeID
                             join l in context.Locations on u.LocationID equals l.LocationID
                             //where (u.CarModel == carmodel) && (u.Manufacturer == manufacturer)
                             select new { u.CarID, v.Manufacturer, v.CarModel,v.PricePerDay,v.PricePerLateDay,v.Gear,u.Kilometrage, l.LocationName,u.isAvailable });
                return query.ToArray();
            }
        }

        public IEnumerable<CarTypes> GetAllCarTypes()
        {
            using (CarDBContext context = new CarDBContext())
            {
                return context.CarTypes.ToArray();
            }
        }

        public CarTypes GetCarType(string manufacturer, string carmodel)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.CarTypes
                             where (u.CarModel == carmodel)&&(u.Manufacturer==manufacturer)
                             select u).FirstOrDefault();

                return (CarTypes)query;
            }
        }

        public CarTypes GetCarTypeByID(int cartypeID)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.CarTypes
                             where (u.CarTypeID == cartypeID)
                             select u).FirstOrDefault();

                return (CarTypes)query;
            }
        }

        public List<CarTypes> GetCarTypes(string manufacturer, string model)
        {
            using (CarDBContext context = new CarDBContext())
            {
                List <CarTypes> l= new List<CarTypes>();
                var query = from ct in context.CarTypes
                             where ct.Manufacturer == manufacturer
                             select ct;

                if (!string.IsNullOrEmpty(model))
                {
                    query = query.Where((ct) => ct.CarModel == model);
                }

                return query.ToList();
            }
        }

        public List<CarTypes> GetCarTypesForManufacturers()
        {
            using (CarDBContext context = new CarDBContext())
            {
                List<CarTypes> l = new List<CarTypes>();
                var query = (from u in context.CarTypes
                             //where (u.Manufacturer == manufacturer)
                             select u.Manufacturer);
                l = (List<CarTypes>)query;
                return l;
            }
        }

        public IEnumerable<Locations> GetAllLocations()
        {
            using (CarDBContext context = new CarDBContext())
            {
                return context.Locations.ToArray();
            }
        }
 
    }
    
}
