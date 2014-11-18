using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace RobotHardwareSoftware
{
    public enum State { 
            
            OFF, /* Machine is Off*/
            READY,
            MOVING, /*Motor Movement*/
            BUSY,   /*Computing Audio*/
            
    }

    public partial class Program
    {
        /* Denotes state of machine*/
        State mcstate;
        GT.StorageDevice SDCard;


        /*Images*/
        Bitmap Rightbitmap, Wrongbitmap;

        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            mcstate = State.OFF;
            initiate_handlers();
        }

        /*************************************************************/
        // Setup eveent handleres
         void initiate_handlers()
         {
                button.ButtonPressed +=new GTM.GHIElectronics.Button.ButtonEventHandler(button_ButtonPressed);
                camera.PictureCaptured +=new Camera.PictureCapturedEventHandler(camera_PictureCaptured);

         }

         void camera_PictureCaptured(GTM.GHIElectronics.Camera sender, GT.Picture picture)
         {
             display_T35.SimpleGraphics.DisplayImage(picture,1,40);

         }
        void  button_ButtonPressed (GTM.GHIElectronics.Button sender, GTM.GHIElectronics.Button.ButtonState state)        
        {
            mcstate = State.READY;    
        }


        void sd_SDCardUnmounted(GTM.GHIElectronics.SDCard sender)
        {
            this.SDCard = null;
        }

        void sd_SDCardMounted(GTM.GHIElectronics.SDCard sender, Gadgeteer.StorageDevice SDCard)
        {
            this.SDCard = SDCard;
        }

        void TeacherLoop()
        {


        }

        /* Notes*/
        void MarkNotes(bool isright)
        {
            if (isright)
            {
                display_T35.SimpleGraphics.DisplayImage(Rightbitmap, 40, 45);
            }
            else
            {
                display_T35.SimpleGraphics.DisplayImage(Wrongbitmap, 40, 45);
            }
         }
        /********* MOTOR  Functions******/
        void moveRadius(int speed)
        { 
        
        }
        void moveHumerus(int speed)
        {
           
        }

        void reset_motors()
        { 
        
        }
    }
}
