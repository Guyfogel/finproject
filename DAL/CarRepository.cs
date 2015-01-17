using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;


namespace DAL
{
    public class CarRepository
    {
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

        public void RemoveUser(string username)
        {
            using (CarDBContext context = new CarDBContext())
            {
                Users user = GetUser(username);
                context.Users.Remove(user);
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

        public Orders GetOrdersByFilter (string carmanufacturer)
        {
            using (CarDBContext context = new CarDBContext())
             {
                var query = (from u in context.Orders
                             where u.Car.CarType.Manufacturer == carmanufacturer
                             select u).FirstOrDefault();

                return (Orders)query;
             }
        }

        public IEnumerable<Users> GetAllUsers()
        {
            using (CarDBContext context = new CarDBContext())
            {
                return context.Users.ToArray();
            }
        }
        //public void AddCar(string carModel, int kilometrage, string location, byte[] photo)
        //{
        //    using (CarDBContext context = new CarDBContext())
        //    {
        //        CarTypes cartype= GetCarType(carModel);
        //        if (cartype == null) return;
        //        Locations Location = GetLocation(location);
        //        if (Location == null) return;
        //            Cars car = new Cars() { CarType = cartype, isAvailable = true, Kilometrage = kilometrage, Location = Location, Photo=photo };
        //            context.Cars.Add(car);
        //            context.SaveChanges();
        //    }
        //}

        public Cars GetCar(string carmodel, string gear, string manufacturer, DateTime Startdate, DateTime Enddate)
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Orders
                             where (u.Car.CarType.Gear == gear)&&(u.Car.CarType.Manufacturer==manufacturer)&&(u.Car.CarType.CarModel==carmodel)&&(u.ActualReturnDate<Startdate)&&(u.LendDate>Enddate)
                             select u.Car).FirstOrDefault();
                return query;
            }
        }
        
        public void AddOrder(Cars car, Users user, DateTime StartDate, DateTime EndDate)
        {
            using (CarDBContext context = new CarDBContext())
            {
                if (car == null) return;
                if (user == null) return;
                Orders order = new Orders() { Car=car, User=user, LendDate=StartDate, ReturnDate=EndDate };
                context.Orders.Add(order);
                context.SaveChanges();
            }

        }

        public Orders GetOrder()
        {
            using (CarDBContext context = new CarDBContext())
            {
                var query = (from u in context.Orders

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

        //public void AddCarType ( string manufacturer , string carModel,decimal pricePerDay,decimal pricePerLateDay,string gear)
        //{
        //    using (CarDBContext context = new CarDBContext())
        //    {
        //        CarTypes CarType = new CarTypes() { Manufacturer = manufacturer, CarModel = carModel, PricePerDay = pricePerDay, PricePerLateDay = pricePerLateDay, Gear = gear };
        //        context.CarTypes.Add(CarType);
        //        context.SaveChanges();
        //    }

        //}

        public void AddCarType(CarTypes cartype)
        {
            using (CarDBContext context = new CarDBContext())
            {
                context.CarTypes.Add(cartype);
                context.SaveChanges();
            }
        }


        public void AddCar(Cars car)
        {
            using (CarDBContext context = new CarDBContext())
            {
                //CarTypes cartype = new CarTypes() { CarModel = car.CarType.CarModel, Gear = car.CarType.Gear, Manufacturer = car.CarType.Manufacturer, PricePerDay = car.CarType.PricePerDay, PricePerLateDay = car.CarType.PricePerLateDay };
                
                //if(GetCarType(cartype.Manufacturer,cartype.CarModel)==null)
                //AddCarType(cartype);
                
                
                context.Cars.Add(car);
                context.CarTypes.Attach(car.CarType);
                context.Locations.Attach(car.Location);
                context.SaveChanges();
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
                             select new { u, v, l });
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
