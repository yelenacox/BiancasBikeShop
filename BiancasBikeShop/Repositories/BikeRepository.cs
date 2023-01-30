using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BiancasBikeShop.Models;
using System.Drawing;
using static Azure.Core.HttpHeader;
using System.ComponentModel;

namespace BiancasBikeShop.Repositories
{
    public class BikeRepository : IBikeRepository
    {
        private SqlConnection Connection
        {
            get
            {
                return new SqlConnection("server=localhost\\SQLExpress;database=BiancasBikeShop;integrated security=true;TrustServerCertificate=true");
            }
        }

        public List<Bike> GetAllBikes()
        {
            // implement code here... 
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT b.Id, b.Brand, b.Color, b.BikeTypeId, b.OwnerId, bt.Id, bt.Name AS BikeTypeName, 
                               o.Id, o.Name AS OwnerName, o.Address, o.Email, o.Telephone
                        FROM Bike b
                        LEFT JOIN BikeType bt on bt.Id = b.BikeTypeId
                        LEFT JOIN Owner o on o.Id = b.OwnerId";

                    List<Bike> bikes = new List<Bike>();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Bike bike = new Bike()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            BikeType = new BikeType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("BikeTypeName"))
                            },
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                            },
                        };
                        bikes.Add(bike);
                    }
                    return bikes;
                }
            }

        }

        public Bike GetBikeById(int id)
        {
            //implement code here...
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT b.Id, b.Brand, b.Color, b.BikeTypeId, b.OwnerId, bt.Id, bt.Name AS BikeTypeName, o.Id, o.Name AS OwnerName, o.Address, o.Email, o.Telephone, 
                        w.Id AS WorkOrderId, w.DateInitiated, w.Description, w.DateCompleted, w.BikeId
                        FROM Bike b
                        LEFT JOIN BikeType bt on bt.Id = b.BikeTypeId
                        LEFT JOIN Owner o on o.Id = b.OwnerId
                        LEFT JOIN WorkOrder w on w.Id = b.Id
                        WHERE b.Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Bike bike = null;
                    if (reader.Read())
                    {
                        bike = new Bike()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            BikeType = new BikeType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("BikeTypeName"))
                            },
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                Telephone = reader.GetString(reader.GetOrdinal("Telephone"))
                            },
                            WorkOrders = new List<WorkOrder>()
                        };
                        if (bike.WorkOrders != null)
                        {
                            bike.WorkOrders.Add(new WorkOrder()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("WorkOrderId")),
                                DateInitiated = reader.GetDateTime(reader.GetOrdinal("DateInitiated")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                DateCompleted = !reader.IsDBNull(reader.GetOrdinal("DateCompleted")) ? reader.GetDateTime(reader.GetOrdinal("DateCompleted")) : null
                            });
                        } else { return null; }
                    }
                        return bike;
                }
            }
        }

        public int GetBikesInShopCount()
        {
            int count = 0;
            // implement code here... 
            return count;

        }
    }
}
