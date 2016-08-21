﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mogre_Advanced_Framework
{
    class DemoApp
    {
        public DemoApp()
        {
            m_pAppStateManager = null;
        }
        ~DemoApp()
        {
            m_pAppStateManager = null;
        }

        public void startDemo()
        {
            new AdvancedMogreFramework();
            if (!AdvancedMogreFramework.initOgre("AdvancedMogreFramework"))
		        return;

            AdvancedMogreFramework.m_pLog.LogMessage("Demo initialized!");
 
	        m_pAppStateManager = new AppStateManager();

            MenuState.create<MenuState>(m_pAppStateManager, "MenuState");
            GameState.create<GameState>(m_pAppStateManager, "GameState");
            PauseState.create<PauseState>(m_pAppStateManager, "PauseState");
 
	        m_pAppStateManager.start(m_pAppStateManager.findByName("MenuState"));
        }

        private AppStateManager m_pAppStateManager;
    }
}