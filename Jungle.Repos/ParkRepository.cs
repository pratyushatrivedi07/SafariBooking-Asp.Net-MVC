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
    public class ParkRepository : IParkRepository
    {
        private MydbContext context;

        public ParkRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(Parks entity)
        {
            try
            {
                //check for duplicate
                var p = context.Parks.FirstOrDefault(e => e.Fee < 10);
                if (p != null)
                {
                    throw new JungleException("Entry Fee should be > 10");
                }
                var park = context.Parks.FirstOrDefault(p => p.Name.ToLower() == entity.Name.ToLower() && p.Location.ToLower() == entity.Location.ToLower());
                if (park != null)
                {
                    throw new JungleException("Park already exists");
                }
                context.Parks.Add(entity);
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
            //catch (DbUpdateException ex)
            //{
            //    throw new JungleException("Department does not exist");
            //}
        }

        public Parks Get(object id)
        {
            try
            {
                Parks park = context.Parks.Find(id);
                return park;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Parks> GetAll()
        {
            try
            {
                return context.Parks.ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Parks> GetByLocation(string location)
        {
            try
            {
                var park = context.Parks.Where(p => p.Location == location).Include(p => p.SafariDetail).ToList();
                return park;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(Parks entity)
        {
            try
            {
                context.Parks.Remove(entity);
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

        public IEnumerable<Parks> Search(string criteria)
        {
            try
            {
                if (int.TryParse(criteria, out int pId))
                {
                    var park = context.Parks.Where(p => p.ParkId == pId).ToList();
                    return park;
                }
                else
                {
                    criteria = criteria.ToLower();
                    var emp = context.Parks.Where(e => e.Name.ToLower().Contains(criteria) || e.Location.ToLower().Contains(criteria)).ToList();
                    return emp;
                }
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Update(Parks entity)
        {
            try
            {
                context.Parks.Update(entity);
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
            throw new NotImplementedException();
        }
    }
}
