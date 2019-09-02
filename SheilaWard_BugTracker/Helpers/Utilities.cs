using SheilaWard_BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SheilaWard_BugTracker.Helpers
{
    public class Utilities
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static string MakeReadable(string property, string value)
        {
            switch (property)
            {
                case "TicketStatusId":
                    return db.TicketStatuses.Find(Convert.ToInt32(value)).Name;
                case "TicketPriorityId":
                    return db.TicketPriorities.Find(Convert.ToInt32(value)).Name;
                case "TicketTypeId":
                    return db.TicketTypes.Find(Convert.ToInt32(value)).Name;
                case "AssignedToUserId":
                case "OwnerUserId":
                    if (!string.IsNullOrEmpty(value))
                    {
                        return db.Users.Find(value).FullName;
                    }
                    return "-- Unassigned --"; 
                default:
                    return value;
            }
        }

        public static string CreateSlug(string fileName)
        {
 
            if (fileName == null) return "";
            const int maxlen = 80;
            int len = fileName.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;
            for (int i = 0; i < len; i++)
            {
                c = fileName[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                            c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if (c == '#')
                {
                    if (i > 0)
                        if (fileName[i - 1] == 'C' || fileName[i - 1] == 'F')
                            sb.Append("-sharp");
                }
                else if (c == '+')
                {
                    sb.Append("-plus");
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (sb.Length == maxlen) break;
            }
            if (prevdash)
            {
                var newString = sb.ToString().Substring(0, sb.Length - 1);
                return sb.ToString().Substring(0, sb.Length - 1);
            }
            else
            {
                var newString = sb.ToString();
                return sb.ToString();
            }
        }
        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            else if ("èéêëę".Contains(s))
            {
                return "e";
            }
            else if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            else if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            else if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            else if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            else if ("żźž".Contains(s))
            {
                return "z";
            }
            else if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            else if ("ñń".Contains(s))
            {
                return "n";
            }
            else if ("ýÿ".Contains(s))
            {
                return "y";
            }
            else if ("ğĝ".Contains(s))
            {
                return "g";
            }
            else if (c == 'ř')
            {
                return "r";
            }
            else if (c == 'ł')
            {
                return "l";
            }
            else if (c == 'đ')
            {
                return "d";
            }
            else if (c == 'ß')
            {
                return "ss";
            }
            else if (c == 'Þ')
            {
                return "th";
            }
            else if (c == 'ĥ')
            {
                return "h";
            }
            else if (c == 'ĵ')
            {
                return "j";
            }
            else
            {
                return "";
            }
        }
    }
}