using Jungle.Entities;
using Jungle.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Jungle.Repos
{
   public class BookingRepository : IBookingRepository
    {
        private MydbContext context;

        public BookingRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(Booking entity)
        {
            try
            {
                context.Booking.Add(entity);
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

        public Booking Get(object id)
        {
            try
            {


                Booking booked = context.Booking.Find(id);
                if(booked != null)
                {
                    return booked;
                }
                else
                {
                    return null;
                }
                

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Booking> GetAll()
        {
            try
            {
                return context.Booking.ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(Booking entity)
        {
            try
            {
                context.Booking.Remove(entity);
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

        public IEnumerable<Booking> Search(int Id)
        {
            try
            { 
                var book = context.Booking.Where(p => p.Id == Id).Include(p => p.P).Include(p => p.Gate).Include(p => p.Safari).Include(p => p.TotalCost).Include(p => p.Vehicle).ToList();
                return book;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Update(Booking entity)
        {
            try
            {

                context.Booking.Update(entity);
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

        public int GetBookingByID()
        {
            int max = context.Booking.Max(b => b.Id);
            return max;
        }

        public IEnumerable<Booking> GetByBookingId(int Id)
        {
            try
            {

                var booking = context.Booking.Where(e => e.Id == Id).ToList();
                return booking;
            }
            catch (SqlException ex)
            {
                throw new JungleException(ex.Message);
            }
        }
    }
}
