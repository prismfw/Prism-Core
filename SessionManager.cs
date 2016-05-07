/*
Copyright (C) 2016  Prism Framework Team

This file is part of the Prism Framework.

The Prism Framework is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

The Prism Framework is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/


using System.Collections.Generic;
using Prism.Native;

#if !DEBUG
using System.Diagnostics;
#endif

namespace Prism
{
    internal static class SessionManager
    {
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly Dictionary<string, Application> sessions = new Dictionary<string, Application>();
#if !DEBUG
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
#endif
        private static readonly INativeSessionManager nativeObject = Application.Resolve<INativeSessionManager>();

        internal static void AbandonSession(string sessionId)
        {
            if (nativeObject != null)
            {
                nativeObject.Abandon(sessionId);
                sessions.Remove(sessionId);
            }
        }

        internal static Application GetCurrentApplication()
        {
            Application value;
            return sessions.TryGetValue(GetCurrentSessionId(), out value) ? value : null;
        }

        internal static string GetCurrentSessionId()
        {
            return nativeObject?.CurrentSessionId ?? string.Empty;
        }

        internal static void SetApplication(Application appInstance)
        {
            string sessionId = GetCurrentSessionId();

            sessions[sessionId] = appInstance;
            appInstance.Session = new SessionSettings(sessionId);
        }
    }
}
