using NamLao206.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace NamLao206.Areas.TransportFiles.Bussiness
{
    public class EmailBussiness
    {
        public static namlao206_websiteEntities db = new namlao206_websiteEntities();
        public static bool DeleteVanBanDen(List<int> ids, out string message)
        {
            var transportsToUpdate = db.Transports
                .Where(t => ids.Contains(t.Id))
                .ToList();

            if (!transportsToUpdate.Any())
            {
                message = "Không tìm thấy văn bản đến để xóa.";
                return false;
            }

            foreach (var transport in transportsToUpdate)
            {
                if (transport.IsActive == true)
                {
                    transport.IsActive = false;
                }
                else
                {
                    db.Transports.Remove(transport);
                }
            }
            db.SaveChanges();
            message = "";
            return true;
        }

        public static bool DeleteVanBanDi(List<int> ids, out string message)
        {
            var transportFilesToUpdate = db.TransportFiles.Include(tf => tf.Transports)
                .Where(tf => ids.Contains(tf.Id) && tf.IsActive == true)
                .ToList();

            if (!transportFilesToUpdate.Any())
            {
                message = "Không tìm thấy văn bản đi để xóa.";
                return false;
            }

            foreach (var transportFile in transportFilesToUpdate)
            {
                if (transportFile.IsActive == true)
                {
                    transportFile.IsActive = false;
                }
                else
                {
                    if (transportFile.Transports != null && transportFile.Transports.Any())
                    {
                        db.Transports.RemoveRange(transportFile.Transports);
                    }

                    var fileUrl = db.TransportFileUrls.FirstOrDefault(x => x.TransportFilesId == transportFile.Id);
                    if (fileUrl != null)
                    {
                        db.TransportFileUrls.Remove(fileUrl);
                    }

                    db.TransportFiles.Remove(transportFile);
                }
            }
            db.SaveChanges();
            message = "";
            return true;
        }

        public static bool DeleteCongVanKhan(List<int> ids, out string message)
        {
            var urgentTransports = db.Transports
                .Include(t => t.TransportFile)
                .Where(t => ids.Contains(t.Id) && t.TransportFile.KhanCap == true)
                .ToList();

            if (!urgentTransports.Any())
            {
                message = "Không tìm thấy công văn khẩn để xóa.";
                return false;
            }

            foreach (var transport in urgentTransports)
            {
                if (transport.IsActive == true)
                {
                    transport.IsActive = false;
                }
                else
                {
                    db.Transports.Remove(transport);
                }
            }
            db.SaveChanges();
            message = "";
            return true;
        }

        public static bool DeleteFromThungRac(List<int> ids, out string message)
        {
            var deletedTransports = db.Transports
                .Include(t => t.TransportFile)
                .Where(t => ids.Contains(t.Id) && t.IsActive == false)
                .ToList();

            if (!deletedTransports.Any())
            {
                message = "Không tìm thấy văn bản trong thùng rác để xóa.";
                return false;
            }

            foreach (var transport in deletedTransports)
            {
                db.Transports.Remove(transport);
            }
            db.SaveChanges();
            message = "";
            return true;
        }
    }
}