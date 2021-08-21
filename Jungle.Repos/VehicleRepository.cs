using Jungle.Entities;
using Jungle.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jungle.Repos
{
    public class VehicleRepository : IVehicleRepository
    {
        private MydbContext context;

        public VehicleRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(Vehicle entity)
        {
            try
            {
                context.Vehicle.Add(entity);
                int recordesAffected = context.SaveChanges();

                if (recordesAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {
                throw new JungleException(ex.Message);
            }
        }

        public Vehicle Get(object id)
        {
            try
            {

                Vehicle vehicle = context.Vehicle.Find(id);
                return vehicle;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Vehicle> GetAll()
        {
            try
            {
                return context.Vehicle.Include(e => e.Park).Where(e=> e.Vtype == "Park").ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Vehicle> GetByPark(int pId)
        {
            try
            {
                var v = context.Vehicle.Where(e => e.ParkId == pId && e.Vtype =="Park").Include(e => e.Park).ToList();
                return v;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(Vehicle entity)
        {
            try
            {
                context.Vehicle.Remove(entity);
                int recordAffected = context.SaveChanges();
                if (recordAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Update(Vehicle entity)
        {
            try
            {
                context.Vehicle.Update(entity);
                int recordAffected = context.SaveChanges();
                if (recordAffected > 0)
                {
                    return true;
                }
                return false;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }
    }
}
