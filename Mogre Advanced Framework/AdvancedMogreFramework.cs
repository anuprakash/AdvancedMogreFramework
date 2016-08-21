﻿using System;
using System.Collections.Generic;
using System.Text;
using Mogre;
using MOIS;
using Mogre_Procedural;
using Mogre_Procedural.MogreBites;

namespace Mogre_Advanced_Framework
{
    class AdvancedMogreFramework 
    {
        public static Root m_pRoot=new Root();
        public static RenderWindow m_pRenderWnd;
        public static Viewport m_pViewport;
        public static Log m_pLog;
        public static Timer m_pTimer;

        public static InputManager m_pInputMgr;
        public static Keyboard m_pKeyboard;
        public static Mouse m_pMouse;

        public static SdkTrayManager m_pTrayMgr;
        public AdvancedMogreFramework()
        {
            m_pRoot = null;
            m_pRenderWnd = null;
            m_pViewport = null;
            m_pLog = null;
            m_pTimer = null;

            m_pInputMgr = null;
            m_pKeyboard = null;
            m_pMouse = null;
            m_pTrayMgr = null;
         }
        ~AdvancedMogreFramework()
        {
            LogManager.Singleton.LogMessage("Shutdown OGRE...");
            if (AdvancedMogreFramework.m_pTrayMgr != null) m_pTrayMgr = null;
            if (AdvancedMogreFramework.m_pInputMgr != null) InputManager.DestroyInputSystem(m_pInputMgr);
            if (AdvancedMogreFramework.m_pRoot != null) m_pRoot = null;
        }

        public static bool initOgre(String wndTitle)
        {
            LogManager logMgr = new LogManager();
 
            m_pLog = LogManager.Singleton.CreateLog("OgreLogfile.log", true, true, false);
            m_pLog.SetDebugOutputEnabled(true);
 
            m_pRoot = new Root();
 
            if(!m_pRoot.ShowConfigDialog())
                return false;
               m_pRenderWnd = m_pRoot.Initialise(true, wndTitle);
 
            m_pViewport = m_pRenderWnd.AddViewport(null);
            ColourValue cv=new ColourValue(0.5f,0.5f,0.5f);
            m_pViewport.BackgroundColour=cv;
 
            m_pViewport.Camera=null;
 
            int hWnd = 0;
            //ParamList paramList;
            m_pRenderWnd.GetCustomAttribute("WINDOW", out hWnd);
 
            m_pInputMgr = InputManager.CreateInputSystem((uint)hWnd);
            m_pKeyboard = (MOIS.Keyboard)m_pInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            m_pMouse =  (MOIS.Mouse)m_pInputMgr.CreateInputObject(MOIS.Type.OISMouse, true);

            m_pMouse.MouseMoved+=new MouseListener.MouseMovedHandler(mouseMoved);
            m_pMouse.MousePressed += new MouseListener.MousePressedHandler(mousePressed);
            m_pMouse.MouseReleased += new MouseListener.MouseReleasedHandler(mouseReleased);

            m_pKeyboard.KeyPressed += new KeyListener.KeyPressedHandler(keyPressed);
            m_pKeyboard.KeyReleased += new KeyListener.KeyReleasedHandler(keyReleased);

            MOIS.MouseState_NativePtr mouseState = m_pMouse.MouseState;
                mouseState.width = m_pViewport.ActualWidth;
                mouseState.height = m_pViewport.ActualHeight;
            //m_pMouse.MouseState = tempMouseState;

 
            String secName, typeName, archName;
            ConfigFile cf=new ConfigFile();
            cf.Load("resources.cfg","\t:=",true);
 
            ConfigFile.SectionIterator seci = cf.GetSectionIterator();
            while (seci.MoveNext())
            {
                secName = seci.CurrentKey;
                ConfigFile.SettingsMultiMap settings = seci.Current;
                foreach (KeyValuePair<string, string> pair in settings)
                {
                    typeName = pair.Key;
                    archName = pair.Value;
                    ResourceGroupManager.Singleton.AddResourceLocation(archName, typeName, secName);
                }
            }
            TextureManager.Singleton.DefaultNumMipmaps=5;
            ResourceGroupManager.Singleton.InitialiseAllResourceGroups(); 
 
            m_pTrayMgr = new SdkTrayManager("AOFTrayMgr", m_pRenderWnd, m_pMouse, null);
 
            m_pTimer = new Timer();
            m_pTimer.Reset();
 
            m_pRenderWnd.IsActive=true;
 
            return true;
        }
        public static void updateOgre(double timeSinceLastFrame)
        {
        }

        public static bool keyPressed(KeyEvent keyEventRef)
        {
             if(m_pKeyboard.IsKeyDown(MOIS.KeyCode.KC_V))
            {
                m_pRenderWnd.WriteContentsToTimestampedFile("AMOF_Screenshot_", ".jpg");
                return true;
            }
 
            if(m_pKeyboard.IsKeyDown(KeyCode.KC_O))
            {
                if(m_pTrayMgr.isLogoVisible())
                {
                    m_pTrayMgr.hideFrameStats();
                    m_pTrayMgr.hideLogo();
                }
                else
                {
                    m_pTrayMgr.showFrameStats(TrayLocation.TL_BOTTOMLEFT);
                    m_pTrayMgr.showLogo(TrayLocation.TL_BOTTOMRIGHT);
                }
            }
 
            return true;
        }
        public static bool keyReleased(KeyEvent keyEventRef)
        {
            return true;
        }

        public static bool mouseMoved(MouseEvent evt)
        {
            return true;
        }
        public static bool mousePressed(MouseEvent evt, MouseButtonID id)
        {
            return true;
        }
        public static bool mouseReleased(MouseEvent evt, MouseButtonID id)
        {
            return true;
        }
        //AdvancedMogreFramework(const AdvancedMogreFramework);
        //AdvancedMogreFramework operator= (const AdvancedMogreFramework);
    }
}