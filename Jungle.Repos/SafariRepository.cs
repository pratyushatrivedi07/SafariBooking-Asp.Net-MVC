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
    public class SafariRepository : ISafariDetailRepos
    {
        private MydbContext context;

        public SafariRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(SafariDetail entity)
        {
            try
            {
                var p = context.SafariDetail.FirstOrDefault(e => e.SafariCost < 100);
                if (p != null)
                {
                    throw new JungleException("Entry Fee should be > 100");
                }
                var safari = context.SafariDetail.FirstOrDefault(e => e.SafariName.ToLower() == entity.SafariName.ToLower() && e.ParkId == entity.ParkId && e.SafariTime.ToLower()==entity.SafariTime.ToLower() && e.SafariDate == entity.SafariDate);
                if (safari != null)
                {
                    throw new JungleException("Safari exists");
                }
                context.SafariDetail.Add(entity);
                int recordsAffected = context.SaveChanges();
                if (recordsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                throw new JungleException("Park does not exist");
            }
        }

        public SafariDetail Get(object id)
        {
            try
            {
                SafariDetail safari = context.SafariDetail.Find(id);
                return safari;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<SafariDetail> GetAll()
        {
            try
            {
                return context.SafariDetail.Include(s => s.Park).ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<SafariDetail> SafariByPark(int parkId)
        {
            try
            {
                var v = context.SafariDetail.Where(e => e.ParkId == parkId).Include(e => e.Park).ToList();
                return v;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(SafariDetail entity)
        {
            try
            {
                context.SafariDetail.Remove(entity);
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
            catch (DbUpdateException ex)
            {
                throw new JungleException("Cannot delete as there as booking against this Safari");
            }
        }

        public IEnumerable<SafariDetail> GetByPark(int parkId)
        {
            try
            {
                var v = context.SafariDetail.Where(e => e.ParkId == parkId).Include(e => e.Park).ToList();
                return v;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Update(SafariDetail entity)
        {
            try
            {
                context.SafariDetail.Update(entity);
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
