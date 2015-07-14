﻿/*
 * FOG Service : A computer management client for the FOG Project
 * Copyright (C) 2014-2015 FOG Project
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 3
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.IO;
using System.Reflection;
using System.Threading;
using FOG.Handlers;
using FOG.Handlers.Power;

namespace FOG
{
    /// <summary>
    ///     Coordinate all user specific FOG modules
    /// </summary>
    internal class main
    {

        private static AbstractService _fogService;
        private const string LogName = "UserService";


        public static void Main(string[] args)
        {
            Thread.Sleep(7*1000);
            //Initialize everything
            Log.FilePath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "fog_user.log");

            AppDomain.CurrentDomain.UnhandledException += Log.UnhandledException;
            Eager.Initalize();

            Log.Entry(LogName, "Initializing");

            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "updating.info")))
            {
                Log.Entry(LogName, "Update.info found, exiting program");
                Power.SpawnUpdateWaiter(Assembly.GetExecutingAssembly().Location);
                Environment.Exit(0);
            }

            _fogService = new FOGUserService();
            _fogService.Start();

            if (Settings.Get("Tray").Equals("1"))
                StartTray();
        }

        private static void StartTray()
        {
            ProcessHandler.RunEXE(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "FOGTray.exe"), "", false);
        }
    }
}