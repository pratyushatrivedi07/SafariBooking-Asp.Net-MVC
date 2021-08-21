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
    public class TouristRepository : ITouristRepository
    {
        private MydbContext context;

        public TouristRepository(MydbContext context)
        {
            this.context = context;
        }

        public bool Add(Tourist entity)
        {
            try
            {
                
                context.Tourist.Add(entity);
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
            
        }

        public Tourist Get(object id)
        {
            try
            {


                Tourist tourist = context.Tourist.Find(id);
                return tourist;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Tourist> GetAll()
        {
            try
            {
                return context.Tourist.ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public Tourist GetTouristByEmail(object emailid)
        {
            try
            {


                Tourist tourist = context.Tourist.FirstOrDefault(e=> e.EmailId == emailid.ToString());
                return tourist;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(Tourist entity)
        {
            try
            {
                context.Tourist.Remove(entity);
                int Isdeleted = context.SaveChanges();
                if (Isdeleted > 0)
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

        public bool Update(Tourist entity)
        {
            try
            {

                context.Tourist.Update(entity);
                int IsUpdated = context.SaveChanges();
                if (IsUpdated > 0)
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
