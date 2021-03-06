﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Drawing;

namespace WhatSUPDesktopApp
{
    /// <summary>
    /// The middleSchoolUserControl is used to separate the top brain from the bottom brain and 
    /// provide information about the functions, healthy behaviors and unhealthy behaviors
    /// </summary>
    public partial class UpstairsBrainControl: UserControl
    {
        private static UpstairsBrainControl _instance; //render userControl based on button pressed

        //flags used to determine which radio button has been used on the editPopup
        bool editFunctionsFlag = false;
        bool editHealthyBehaviorsFlag = false;
        bool editUnhealthyBehaviorsFlag = false;

        private object movingObject; //The image being dragged and dropped 
        private double firstXPos, firstYPos;
       
        //used to save the original position of the brain needed for the reset button 
        //functionality
        double originalFrontalLobeX;
        double originalFrontalLobeY;
        double originalOccipitalLobeX;
        double originalOccipitalLobeY;
        double originalParietalLobeX;
        double originalParietalLobeY;
        double originalTemporalLobeX;
        double originalTemporalLobeY;
        double originalCerebellumX;
        double originalCerebellumY;

        Storyboard sbFL; //Storyboard for the frontal lobe
        Storyboard sbTL; //Storyboard for the temporal lobe
        Storyboard sbPL; // storyboard for the parietal lobe
        Storyboard sbOL; //storyboard for the occipital lobe 
        Storyboard sbC;  //storyboard for the cerebellum
        //1-5 are used to determine which storyboard should be affected by the action
        //sbFL = 1, sbTL = 2, sbPL = 3, sbOL = 4, sbC = 5
        int storyboardFlag;

        string brainPart; //used to save the selection of the drop down list in editPopup

        bool disabled = false;//used to distinguish between single and double click in same function
        
        string changedFactsMessageBoxText;//used to save content of textbox in editPopup

        //Used to initialize text display for the functions, healthy behaviors, and unhealthy behaviors for Frontal Lobe
        string changedFunctionsFrontalLobe = null;
        string defaultFunctionsFrontalLobe = Properties.Settings.Default.defaultFunctionsFrontalLobe;
        bool defaultFunctionsFLFlag = true;
        string defaultHealthyBehaviorsFrontalLobe = Properties.Settings.Default.defaultHealthyBehaviorsFrontalLobe;
        string changedHealthyBehaviorsFrontalLobe = null;
        bool defaultHBFLFlag = true;
        string changedUnhealthyBehaviorsFrontalLobe = null;
        string defaultUnhealthyBehaviorsFrontalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsFrontalLobe;
        bool defaultUHBFLFlag = true;

        //Used to initialize text display for the functions, healthy behaviors, and unhealthy behaviors for Temporal Lobe
        string changedFunctionsTemporalLobe = null;
        string defaultFunctionsTemporalLobe = Properties.Settings.Default.defaultFunctionsTemporalLobe;
        bool defaultFunctionsTLFlag=true;
        string changedHealthyBehaviorsTemporalLobe = null;
        string defaultHealthyBehaviorsTemporalLobe = Properties.Settings.Default.defaultHealthyBehaviorsTemporalLobe;
        bool defaultHBTLFlag = true;
        string changedUnhealthyBehaviorsTemporalLobe = null;
        string defaultUnhealthyBehaviorsTemporalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsTemporalLobe;
        bool defaultUHBTLFlag = true;

        //Used to initialize text display for the functions, healthy behaviors, and unhealthy behaviors for Parietal Lobe
        string changedFunctionsParietalLobe = null;
        string defaultFunctionsParietalLobe = Properties.Settings.Default.defaultFunctionsParietalLobe;
        bool defaultFunctionsPLFlag = true;
        string changedHealthyBehaviorsParietalLobe = null;
        string defaultHealthyBehaviorsParietalLobe = Properties.Settings.Default.defaultHealthyBehaviorsParietalLobe;
        bool defaultHBPLFlag = true;
        string changedUnhealthyBehaviorsParietalLobe = null;
        string defaultUnhealthyBehaviorsParietalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsParietalLobe;
        bool defaultUHBPLFlag = true;

        //Used to initialize text display for the functions, healthy behaviors, and unhealthy behaviors for Occipital Lobe
        string changedFunctionsOccipitalLobe = null;
        string defaultFunctionsOccipitalLobe = Properties.Settings.Default.defaultFunctionsOccipitalLobe;
        bool defaultFunctionsOLFlag = true;
        string changedHealthyBehaviorsOccipitalLobe = null;
        string defaultHealthyBehaviorsOccipitalLobe = Properties.Settings.Default.defaultHealthyBehaviorsOccipitalLobe;
        bool defaultHBOLFlag = true;
        string changedUnhealthyBehaviorsOccipitalLobe = null;
        string defaultUnhealthyBehaviorsOccipitalLobe = Properties.Settings.Default.defaultUnhealthyBehaviorsOccipitalLobe;
        bool defaultUHBOLFlag = true;

        ////Used to initialize text display for the functions, healthy behaviors, and unhealthy behaviors for Cerebellum
        string changedFunctionsCerebellum = null;
        string defaultFunctionsCerebellum = Properties.Settings.Default.defaultFunctionsCerebellum;
        bool defaultFunctionsCFlag = true;
        string changedHealthyBehaviorsCerebellum = null;
        string defaultHealthyBehaviorsCerebellum = Properties.Settings.Default.defaultHealthyBehaviorsCerebellum;
        bool defaultHBCFlag = true;
        string changedUnhealthyBehaviorsCerebellum = null;
        string defaultUnhealthyBehaviorsCerebellum = Properties.Settings.Default.defaultUnhealthyBehaviorsCerebellum;
        bool defaultUHBCFlag = true;



        //This method allows the middle school UserControl to be rendered when called
        public static UpstairsBrainControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UpstairsBrainControl();
                }
                _instance = new UpstairsBrainControl();
                return _instance;

            }
        }

        //This is the entry point for the userControl page 
        //This function initializes the userControl with EventHandlers and other characteristics
        //EventHandlers are routed to controls in xaml file
        //Bind animations for brain parts to declared and defined storyboards in the xaml file 
        public UpstairsBrainControl()
        {
            InitializeComponent();
   
            //Event handlers for the images and gotItButton
            frontalLobeBox.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(FrontalLobeBox_MouseDown);
            frontalLobeBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            frontalLobeBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            temporalLobeBox.MouseDown += new MouseButtonEventHandler(TemporalLobeBox_MouseDown);
            temporalLobeBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            temporalLobeBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            parietalLobeBox.MouseDown += new MouseButtonEventHandler(ParietalLobeBox_MouseDown);
            parietalLobeBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            parietalLobeBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            occipitalLobeBox.MouseDown += new MouseButtonEventHandler(OccipitalLobeBox_MouseDown);
            occipitalLobeBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            occipitalLobeBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            cerebellumBox.MouseDown += new MouseButtonEventHandler(CerebellumBox_MouseDown);
            cerebellumBox.PreviewMouseMove += new MouseEventHandler(Img_PreviewMouseMove);
            cerebellumBox.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Img_PreviewMouseLeftButtonUp);

            gotItButton.Click += new RoutedEventHandler(GotItButton_Click);


            

            //Saving the original positions of the brain parts
            originalFrontalLobeX = Canvas.GetLeft((UIElement)frontalLobeBox);
            originalFrontalLobeY = Canvas.GetTop((UIElement)frontalLobeBox);

            originalParietalLobeX = Canvas.GetLeft((UIElement)parietalLobeBox);
            originalParietalLobeY = Canvas.GetTop((UIElement)parietalLobeBox);

            originalOccipitalLobeX = Canvas.GetLeft((UIElement)occipitalLobeBox);
            originalOccipitalLobeY = Canvas.GetTop((UIElement)occipitalLobeBox);

            originalTemporalLobeX = Canvas.GetLeft((UIElement)temporalLobeBox);
            originalTemporalLobeY = Canvas.GetTop((UIElement)temporalLobeBox);

            originalCerebellumX = Canvas.GetLeft((UIElement)cerebellumBox);
            originalCerebellumY = Canvas.GetTop((UIElement)cerebellumBox);

            //Binding the declared storyboards(in code behind) to storyboards in user control resources in(xaml)
            sbFL = (Storyboard)this.Resources["zoomingFL"];
            sbTL = (Storyboard)this.Resources["zoomingTL"];
            sbPL = (Storyboard)this.Resources["zoomingPL"];
            sbOL = (Storyboard)this.Resources["zoomingOL"];
            sbC = (Storyboard)this.Resources["zoomingC"];
            storyboardFlag = 0; // Initiates storyboard flag at zero



        }
        //When functions radio button is chosen
        //This function sets functions flag to 
        //true and healthyBehaviors and unhealthybehaviors to false
        public void Functions_Checked(object sender, RoutedEventArgs e)
        {
            factsMessageBox.Text = "";
            WhichFunction();
        }

        //This function is used to determine which brain's part functions to display
        //If default function flag is true use functions from settings 
        //else use edited functions for each part then save as default in settings
        //the storyboard flag determine which brain part

        private void WhichFunction() {

            switch (storyboardFlag)
            {
                case 1:
                    if (defaultFunctionsFLFlag)
                    {
                        factsMessageBox.Text = "Functions of the Frontal Lobe: " + Properties.Settings.Default.defaultFunctionsFrontalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Functions of the Frontal Lobe: " + changedFunctionsFrontalLobe;
                        Properties.Settings.Default.defaultFunctionsFrontalLobe = changedFunctionsFrontalLobe;
                        Properties.Settings.Default.Save();
                        defaultFunctionsFLFlag = true;
                    }
                    break;
                case 2:
                    if (defaultFunctionsTLFlag)
                    {
                        factsMessageBox.Text = "Functions of the Temporal Lobe: " + Properties.Settings.Default.defaultFunctionsTemporalLobe;

                    }
                    else
                    {
                        factsMessageBox.Text = "Functions of Temporal Lobe: " + changedFunctionsTemporalLobe;
                        Properties.Settings.Default.defaultFunctionsTemporalLobe = changedFunctionsTemporalLobe;
                        Properties.Settings.Default.Save();
                        defaultFunctionsTLFlag = true;
                    }
                    break;
                case 3:
                    if (defaultFunctionsPLFlag)
                    {
                        factsMessageBox.Text = "Functions of the Parietal Lobe: " + Properties.Settings.Default.defaultFunctionsParietalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Functions of the Parietal Lobe: " + changedFunctionsParietalLobe;
                        Properties.Settings.Default.defaultFunctionsParietalLobe = changedFunctionsParietalLobe;
                        Properties.Settings.Default.Save();
                        defaultFunctionsPLFlag = true;
                    }
                    break;
                case 4:
                    if (defaultFunctionsOLFlag)
                    {
                        factsMessageBox.Text = "Functions of the Occipital Lobe: " + Properties.Settings.Default.defaultFunctionsOccipitalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Functions of the Occipital Lobe: " + changedFunctionsOccipitalLobe;
                        Properties.Settings.Default.defaultFunctionsOccipitalLobe = changedFunctionsOccipitalLobe;
                        Properties.Settings.Default.Save();
                        defaultFunctionsOLFlag = true;
                    }
                    break;
                case 5:
                    if (defaultFunctionsCFlag)
                    {
                        factsMessageBox.Text = "Functions of the Cerebellum: " + Properties.Settings.Default.defaultFunctionsCerebellum;
                    }
                    else
                    {
                        factsMessageBox.Text = "Functions of the Cerebellum: " + changedFunctionsCerebellum;
                        Properties.Settings.Default.defaultFunctionsCerebellum = changedFunctionsCerebellum;
                        Properties.Settings.Default.Save();
                        defaultFunctionsCFlag = true;
                    }
                    break;
            }

        }

        //This function is called when the healthy behaviors radio button is selected 
        //It calls the function that determines which brain part's healthy behaviors to display
        private void HealthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {

            factsMessageBox.Text = "";
            WhichHealthyBehavior();
        }

        //This function determines which brain part's healthy behavior to display
        //If default healthy behavior flag is true use healthy behaviors from settings 
        //else use edited healthy behaviors for each part then save as default in settings
        //the storyboard flag determine which brain part
        private void WhichHealthyBehavior() {
            switch (storyboardFlag)
            {

                case 1:
                    if (defaultHBFLFlag)
                    {
                        factsMessageBox.Text = "Examples Healthy Behaviors that affect the Frontal Lobe: " + defaultHealthyBehaviorsFrontalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples Healthy Behaviors that affect the Frontal Lobe: " + changedHealthyBehaviorsFrontalLobe;
                        Properties.Settings.Default.defaultHealthyBehaviorsFrontalLobe = changedHealthyBehaviorsFrontalLobe;
                        Properties.Settings.Default.Save();
                        defaultHBFLFlag = true;
                    }

                    break;
                case 2:
                    if (defaultHBTLFlag)
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Temporal Lobe: " + Properties.Settings.Default.defaultHealthyBehaviorsTemporalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Temporal Lobe: " + changedHealthyBehaviorsTemporalLobe;
                        Properties.Settings.Default.defaultHealthyBehaviorsTemporalLobe = changedHealthyBehaviorsTemporalLobe;
                        Properties.Settings.Default.Save();
                        defaultHBTLFlag = true;
                    }

                    break;
                case 3:
                    if (defaultHBPLFlag)
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Parietal Lobe: " + Properties.Settings.Default.defaultHealthyBehaviorsParietalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Parietal Lobe: " + changedHealthyBehaviorsParietalLobe;
                        Properties.Settings.Default.defaultHealthyBehaviorsParietalLobe = changedHealthyBehaviorsParietalLobe;
                        Properties.Settings.Default.Save();
                        defaultHBPLFlag = true;
                    }
                    break;
                case 4:
                    if (defaultHBOLFlag)
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Occipital Lobe: " + Properties.Settings.Default.defaultHealthyBehaviorsOccipitalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Occipital Lobe: " + changedHealthyBehaviorsOccipitalLobe;
                        Properties.Settings.Default.defaultHealthyBehaviorsOccipitalLobe = changedHealthyBehaviorsOccipitalLobe;
                        Properties.Settings.Default.Save();
                        defaultHBOLFlag = true;
                    }
                    break;
                case 5:
                    if (defaultHBCFlag)
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Cerebellum: " + Properties.Settings.Default.defaultHealthyBehaviorsCerebellum;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of Healthy Behaviors that affect the Cerebellum: " + changedHealthyBehaviorsCerebellum;
                        Properties.Settings.Default.defaultHealthyBehaviorsCerebellum = changedHealthyBehaviorsCerebellum;
                        Properties.Settings.Default.Save();
                        defaultHBCFlag = true;
                    }
                    break;
            }

        }
        //This function is called when the unhealthy behaviors radio button is selected 
        //It calls the function that determines which brain part's unhealthy behaviors to display
        private void UnhealthyBehaviors_Checked(object sender, RoutedEventArgs e)
        {

            factsMessageBox.Text = "";
            WhichUnhealthyBehavior();
        }
        //This function is used to determine which brain part's unhealthy behaviors 
        //If default unhealthy behavior flag is true use unhealthy behaviors from settings 
        //else use edited unhealthy behaviors for each part then save as default in settings
        //the storyboard flag determine which brain part
        private void WhichUnhealthyBehavior() {
            switch (storyboardFlag)
            {
                case 1:
                    if (defaultUHBFLFlag)
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Frontal Lobe : " + Properties.Settings.Default.defaultUnhealthyBehaviorsFrontalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Frontal Lobe: " + changedUnhealthyBehaviorsFrontalLobe;
                        Properties.Settings.Default.defaultUnhealthyBehaviorsFrontalLobe = changedUnhealthyBehaviorsFrontalLobe;
                        Properties.Settings.Default.Save();
                        defaultUHBFLFlag = true;
                    }

                    break;
                case 2:
                    if (defaultUHBTLFlag)
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Temporal Lobe: " + Properties.Settings.Default.defaultUnhealthyBehaviorsTemporalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Temporal Lobe: " + changedUnhealthyBehaviorsTemporalLobe;
                        Properties.Settings.Default.defaultUnhealthyBehaviorsTemporalLobe = changedUnhealthyBehaviorsTemporalLobe;
                        Properties.Settings.Default.Save();
                        defaultUHBTLFlag = true;
                    }

                    break;
                case 3:
                    if (defaultUHBPLFlag)
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Parietal Lobe: " + Properties.Settings.Default.defaultUnhealthyBehaviorsParietalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Parietal Lobe: " + changedUnhealthyBehaviorsParietalLobe;
                        Properties.Settings.Default.defaultUnhealthyBehaviorsParietalLobe = changedUnhealthyBehaviorsParietalLobe;
                        Properties.Settings.Default.Save();
                        defaultUHBPLFlag = true;
                    }
                    break;
                case 4:
                    if (defaultUHBOLFlag)
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Occipital Lobe: " + Properties.Settings.Default.defaultUnhealthyBehaviorsOccipitalLobe;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Occipital Lobe: " + changedUnhealthyBehaviorsOccipitalLobe;
                        Properties.Settings.Default.defaultUnhealthyBehaviorsOccipitalLobe = changedUnhealthyBehaviorsOccipitalLobe;
                        Properties.Settings.Default.Save();
                        defaultUHBOLFlag = true;
                    }
                    break;
                case 5:
                    if (defaultUHBCFlag)
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Cerebellum: " + Properties.Settings.Default.defaultUnhealthyBehaviorsCerebellum;
                    }
                    else
                    {
                        factsMessageBox.Text = "Examples of ways Unhealthy Behaviors affect the Cerebellum: " + changedUnhealthyBehaviorsCerebellum;
                        Properties.Settings.Default.defaultUnhealthyBehaviorsCerebellum = changedUnhealthyBehaviorsCerebellum;
                        Properties.Settings.Default.Save();
                        defaultUHBCFlag = true;
                    }
                    break;
            }


        }
        //This function is called when the edit button is clicked
        //It opens the edit popup
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // open the Popup if it isn't open already 
            if (!editPopup.IsOpen)
            {
                editPopup.IsOpen = true;

            }
        }

        //This function is called when the editing textbox text changes. 
        //Saves the changes into a global variable that can be used 
        //to be saved as a default
        private void CloseEditPopupClicked(object sender, RoutedEventArgs e)
        {
            // if the Popup is open, then close it 
            //reset the popup
            if (editPopup.IsOpen) { editPopup.IsOpen = false; }
            editingTextBox.Text = "";
            editFunctionsRadioButton.IsChecked = false;
            editFunctionsFlag = false;
            editHealthyBehaviorsRadioButton.IsChecked = false;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsRadioButton.IsChecked = false;
            editUnhealthyBehaviorsFlag = false;
            brainPartComboBox.SelectedItem = null;
            editingTextBox.IsReadOnly = true;

        }
        //This function saves the editingTextBox text into a variable to be saved 
        //as default setting later
        private void EditingTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            changedFactsMessageBoxText = editingTextBox.Text;
        }
        //This function is to save the selected brain part from the drop down list in the 
        //editPopup into a string to determine the correct message to display when saving
        private void BrainPartComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                brainPart = ((ComboBoxItem)brainPartComboBox.SelectedItem).Content.ToString();
            }
            catch
            {
                editingTextBox.Text = "Select a brain part from the dropdown list";
            }
        }

        //This function is called when the functions radio button of the editing button is selected
        private void EditFunctionsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = true;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsFlag = false;
            editingTextBox.Text = "";
            if (brainPart != null)
            {
                editingTextBox.IsReadOnly = false;
                switch (brainPart)
                {
                    case "Cerebellum":
                        editingTextBox.Text = defaultFunctionsCerebellum;
                        break;
                    case "Occipital Lobe":
                        editingTextBox.Text = defaultFunctionsOccipitalLobe;
                        break;
                    case "Parietal Lobe":
                        editingTextBox.Text = defaultFunctionsParietalLobe;
                        break;
                    case "Temporal Lobe":
                        editingTextBox.Text = defaultFunctionsTemporalLobe;
                        break;
                    case "Frontal Lobe":
                        editingTextBox.Text = defaultFunctionsFrontalLobe;

                        break;

                }
            }
        }
        //This function is called when the healthy behavior radio button of the editing pop up
        private void EditHealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = false;
            editHealthyBehaviorsFlag = true;
            editUnhealthyBehaviorsFlag = false;
            editingTextBox.Text = "";
            if (brainPart != null)
            {
                editingTextBox.IsReadOnly = false;
                switch (brainPart)
                {
                    case "Cerebellum":
                        editingTextBox.Text = defaultHealthyBehaviorsCerebellum;
                        break;
                    case "Occipital Lobe":
                        editingTextBox.Text = defaultHealthyBehaviorsOccipitalLobe;
                        break;
                    case "Parietal Lobe":
                        editingTextBox.Text = defaultHealthyBehaviorsParietalLobe;
                        break;
                    case "Temporal Lobe":
                        editingTextBox.Text = defaultHealthyBehaviorsTemporalLobe;
                        break;
                    case "Frontal Lobe":
                        editingTextBox.Text = defaultHealthyBehaviorsFrontalLobe;

                        break;

                }
            }
        }
        //This function is called when the unhealthy behaviors radio button is checked 
        private void EditUnhealthyBehaviorsRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            editFunctionsFlag = false;
            editHealthyBehaviorsFlag = false;
            editUnhealthyBehaviorsFlag = true;
            editingTextBox.Text = "";
            if (brainPart != null)
            {
                editingTextBox.IsReadOnly = false;
                switch (brainPart)
                {
                    case "Cerebellum":
                        editingTextBox.Text = defaultUnhealthyBehaviorsCerebellum;
                        break;
                    case "Occipital Lobe":
                        editingTextBox.Text = defaultUnhealthyBehaviorsOccipitalLobe;
                        break;
                    case "Parietal Lobe":
                        editingTextBox.Text = defaultUnhealthyBehaviorsParietalLobe;
                        break;
                    case "Temporal Lobe":
                        editingTextBox.Text = defaultUnhealthyBehaviorsTemporalLobe;
                        break;
                    case "Frontal Lobe":
                        editingTextBox.Text = defaultUnhealthyBehaviorsFrontalLobe;

                        break;

                }
            }
        }
        //This function is called when the save button of the edit pop up is clicked 
        //What the editingMessageBlcok.Text displays depend on the brain part selected and 
        //the radio button selected. the editingTextbox isn't read only when both have been selected
        //Text for the currrent selection displays before being editable

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (brainPart == null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            {
                editingMessageBlock.Text = "Choose Brain Part and either functions, healthy behaviors and unhealthy behaviors";
            }
            if (brainPart == null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            {
                editingMessageBlock.Text = "Choose Brain Part";
            }

            if (brainPart != null && !(editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            { editingMessageBlock.Text = "Choose either functions, healthy behaviors, and unhealthy behaviors"; }
            if (brainPart != null && (editFunctionsFlag || editHealthyBehaviorsFlag || editUnhealthyBehaviorsFlag))
            {
                editingMessageBlock.Text = "Saved";
                editingTextBox.IsReadOnly = false;
                if (editFunctionsFlag)
                {
                    switch (brainPart)
                    {
                        case "Cerebellum":
                            changedFunctionsCerebellum = changedFactsMessageBoxText;
                            defaultFunctionsCFlag = false;
                            break;
                        case "Occipital Lobe":
                            changedFunctionsOccipitalLobe = changedFactsMessageBoxText;
                            defaultFunctionsOLFlag = false;
                            break;
                        case "Parietal Lobe":
                            changedFunctionsParietalLobe = changedFactsMessageBoxText;
                            defaultFunctionsPLFlag = false;
                            break;
                        case "Temporal Lobe":
                            changedFunctionsTemporalLobe = changedFactsMessageBoxText;
                            defaultFunctionsTLFlag = false;
                            break;
                        case "Frontal Lobe":
                            changedFunctionsFrontalLobe = changedFactsMessageBoxText;
                            defaultFunctionsFLFlag = false;
                            break;

                    }
                }

                if (editHealthyBehaviorsFlag)
                {
                    switch (brainPart)
                    {
                        case "Cerebellum":

                            changedHealthyBehaviorsCerebellum = changedFactsMessageBoxText;
                            defaultHBCFlag = false;
                            break;
                        case "Occipital Lobe":
                            changedHealthyBehaviorsOccipitalLobe = changedFactsMessageBoxText;
                            defaultHBOLFlag = false;
                            break;
                        case "Parietal Lobe":
                            changedHealthyBehaviorsParietalLobe = changedFactsMessageBoxText;
                            defaultHBPLFlag = false;
                            break;
                        case "Temporal Lobe":
                            changedHealthyBehaviorsTemporalLobe = changedFactsMessageBoxText;
                            defaultHBTLFlag = false;
                            break;
                        case "Frontal Lobe":

                            changedHealthyBehaviorsFrontalLobe = changedFactsMessageBoxText;
                            defaultHBFLFlag = false;
                            break;
                    }
                }
                if (editUnhealthyBehaviorsFlag)
                {
                    switch (brainPart)
                    {
                        case "Cerebellum":
                            changedUnhealthyBehaviorsCerebellum = changedFactsMessageBoxText;
                            defaultUHBCFlag = false;

                            break;
                        case "Occipital Lobe":
                            changedUnhealthyBehaviorsOccipitalLobe = changedFactsMessageBoxText;
                            defaultUHBOLFlag = false;
                            break;
                        case "Parietal Lobe":
                            changedUnhealthyBehaviorsParietalLobe = changedFactsMessageBoxText;
                            defaultUHBPLFlag = false;
                            break;
                        case "Temporal Lobe":
                            changedUnhealthyBehaviorsTemporalLobe = changedFactsMessageBoxText;
                            defaultUHBTLFlag = false;
                            break;
                        case "Frontal Lobe":
                            changedUnhealthyBehaviorsFrontalLobe = changedFactsMessageBoxText;
                            defaultUHBFLFlag = false;
                            break;
                    }

                }
            }
        }
        
        //This functions determines what happens when the frontalLobe box is clicked once with any 
        //mouse button
        #region
        private void FrontalLobeBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storyboardFlag = 1;
            if (e.ClickCount == 1 && !disabled)
            {
                brainPartsLabel.Text = "Frontal Lobe"; //Label for the brain part

                //Prepares the image to be dragged and dropped
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                firstXPos = e.GetPosition(img).X;
                firstYPos = e.GetPosition(img).Y;
                movingObject = sender;

                // Put the image currently being dragged on top of the others
                int top = Canvas.GetZIndex(img);
                foreach (System.Windows.Controls.Image child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
                Canvas.SetZIndex(img, top + 1);
                Mouse.Capture(img); //So the mouse pointer doesn't "slip" off of image while dragging and dropping 
    
            }
            //This  determines what happens when the hippocampus is clicked on with the mouse's left button
            if (e.ClickCount == 2)
            {
                //The following determines what to be displayed in factsMessageBox based on
                //the selected radio button 
                if (functions.IsChecked == true) {
                    WhichFunction();
                }
                if (healthyBehaviors.IsChecked==true) {
                    WhichHealthyBehavior();
                }
                if (unhealthyBehaviors.IsChecked == true) {
                    WhichUnhealthyBehavior();
                }

                //without reverseOn every other time autoreverse = true
                if (sbFL.AutoReverse)
                    ReverseOff(1);
                else { ReverseOn(1); }

                //Hide other parts and make facts box and go button visible
                frontalLobeBox.SetValue(Canvas.LeftProperty, originalFrontalLobeX);
                frontalLobeBox.SetValue(Canvas.TopProperty, originalFrontalLobeY);
                parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
                occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                cutoutUpstairsBrain.Visibility = System.Windows.Visibility.Hidden;
                resetButton.Visibility = System.Windows.Visibility.Hidden;
                factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                gotItButton.Visibility = System.Windows.Visibility.Visible;
                brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                disabled = true;

            }
        }
        #endregion

        //This functions determines what happens when the temporalLobe box is clicked once with any 
        //mouse button
        #region
        private void TemporalLobeBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storyboardFlag = 2;
            if (e.ClickCount == 1 && !disabled)
            {
                brainPartsLabel.Text = "Temporal Lobe";
                // In this event, we get the current mouse position on the control to use it in the MouseMove event.
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                firstXPos = e.GetPosition(img).X;
                firstYPos = e.GetPosition(img).Y;
                movingObject = sender;

                // Put the image currently being dragged on top of the others
                int top = Canvas.GetZIndex(img);
                foreach (System.Windows.Controls.Image child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
                Canvas.SetZIndex(img, top + 1);
                Mouse.Capture(img); //So the mouse pointer doesn't "slip" off image when being dragged and dropped 

                
                
        
            }
            //This function determines what happens when the hippocampus is clicked on with the mouse's left button
            if (e.ClickCount == 2)
            {   //The following determines what to be displayed in factsMessageBox based on
                //the selected radio button 
                if (functions.IsChecked == true)
                {
                    WhichFunction();
                }
                if (healthyBehaviors.IsChecked == true)
                {
                    WhichHealthyBehavior();
                }
                if (unhealthyBehaviors.IsChecked == true)
                {
                    WhichUnhealthyBehavior();
                }

                //without reverseOn every other time autoreverse = true
                if (sbTL.AutoReverse)
                {
                    ReverseOff(2);
                }
                else
                {
                    ReverseOn(2);
                }
                //Hide other parts and make facts box and go button visible
                temporalLobeBox.SetValue(Canvas.LeftProperty, originalTemporalLobeX);
                temporalLobeBox.SetValue(Canvas.TopProperty, originalTemporalLobeY);
                parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
                occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                resetButton.Visibility = System.Windows.Visibility.Hidden;
                cutoutUpstairsBrain.Visibility = System.Windows.Visibility.Hidden;
                factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                gotItButton.Visibility = System.Windows.Visibility.Visible;
                brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                disabled = true;
            }
        }
        #endregion

        //This functions determines what happens when the parietalLobe box is clicked once with any 
        //mouse button
        #region
        private void ParietalLobeBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storyboardFlag = 3;
            if (e.ClickCount == 1 && !disabled)
            {
                brainPartsLabel.Text = "Parietal Lobe";
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                firstXPos = e.GetPosition(img).X;
                firstYPos = e.GetPosition(img).Y;
                movingObject = sender;

                // Put the image currently being dragged on top of the others
                int top = Canvas.GetZIndex(img);
                foreach (System.Windows.Controls.Image child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
                Canvas.SetZIndex(img, top + 1);
                Mouse.Capture(img); //So the mouse pointer doesn't "slip" off image when being dragged and dropped 

             
               
            }
            //This function determines what happens when the hippocampus is clicked on with the mouse's left button
            if (e.ClickCount == 2)
            {
                //without reverseOn every other time autoreverse = true
                if (sbPL.AutoReverse)
                {
                    ReverseOff(3);
                }
                else
                {
                    ReverseOn(3);
                }
                //The following determines what to be displayed in factsMessageBox based on
                //the selected radio button 
                if (functions.IsChecked == true)
                {
                    WhichFunction();
                }
                if (healthyBehaviors.IsChecked == true)
                {
                    WhichHealthyBehavior();
                }
                if (unhealthyBehaviors.IsChecked == true)
                {
                    WhichUnhealthyBehavior();
                }
                //Hide other parts and make facts box and go button visible
                parietalLobeBox.SetValue(Canvas.LeftProperty, originalParietalLobeX);
                parietalLobeBox.SetValue(Canvas.TopProperty, originalParietalLobeY);
                temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
                occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                resetButton.Visibility = System.Windows.Visibility.Hidden;
                cutoutUpstairsBrain.Visibility = System.Windows.Visibility.Hidden;
                factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                gotItButton.Visibility = System.Windows.Visibility.Visible;
                brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                disabled = true;
            }
        }
        #endregion
        //This functions determines what happens when the occipitalLobe box is clicked once with any 
        //mouse button
        #region
        private void OccipitalLobeBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storyboardFlag = 4;
            if (e.ClickCount == 1 && !disabled)
            {
                brainPartsLabel.Text = "Occipital Lobe";
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                firstXPos = e.GetPosition(img).X;
                firstYPos = e.GetPosition(img).Y;
                movingObject = sender;

                // Put the image currently being dragged on top of the others
                int top = Canvas.GetZIndex(img);
                foreach (System.Windows.Controls.Image child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
                Canvas.SetZIndex(img, top + 1);
                Mouse.Capture(img); //So the mouse pointer doesn't "slip" off image when being dragged and dropped 

            }
            //This function determines what happens when the hippocampus is clicked on with the mouse's left button
            if (e.ClickCount == 2)
            {
                //without reverseOn every other time autoreverse = true
                if (sbOL.AutoReverse)
                {
                    ReverseOff(4);
                }
                else
                {
                    ReverseOn(4);
                }
                //The following determines what to be displayed in factsMessageBox based on
                //the selected radio button 
                if (functions.IsChecked == true)
                {
                    WhichFunction();
                }
                if (healthyBehaviors.IsChecked == true)
                {
                    WhichHealthyBehavior();
                }
                if (unhealthyBehaviors.IsChecked == true)
                {
                    WhichUnhealthyBehavior();
                }
                //Hide other parts and make facts box and go button visible
                occipitalLobeBox.SetValue(Canvas.LeftProperty, originalOccipitalLobeX);
                occipitalLobeBox.SetValue(Canvas.TopProperty, originalOccipitalLobeY);
                temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                cerebellumBox.Visibility = System.Windows.Visibility.Hidden;
                parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                resetButton.Visibility = System.Windows.Visibility.Hidden;
                cutoutUpstairsBrain.Visibility = System.Windows.Visibility.Hidden;
                factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                gotItButton.Visibility = System.Windows.Visibility.Visible;
                brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                disabled = true;
            }
        }
        #endregion

        //This functions determines what happens when the cerebellum box is clicked once with any 
        //mouse button
        #region
        private void CerebellumBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            storyboardFlag = 5;
            if (e.ClickCount == 1 && !disabled)
            {
                brainPartsLabel.Text = "Cerebellum";
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                firstXPos = e.GetPosition(img).X;
                firstYPos = e.GetPosition(img).Y;
                movingObject = sender;

                // Put the image currently being dragged on top of the others
                int top = Canvas.GetZIndex(img);
                foreach (System.Windows.Controls.Image child in canvas.Children)
                    if (top < Canvas.GetZIndex(child))
                        top = Canvas.GetZIndex(child);
                Canvas.SetZIndex(img, top + 1);
                Mouse.Capture(img); //So the mouse pointer doesn't "slip" off of image while dragging and dropping 

            }
            //This function determines what happens when the hippocampus is clicked on with the mouse's left button
            if (e.ClickCount == 2)
            {
                //without reverseOn every other time autoreverse = true
                if (sbC.AutoReverse)
                {
                    ReverseOff(5);
                }
                else
                {
                    ReverseOn(5);
                }
                //The following determines what to be displayed in factsMessageBox based on
                //the selected radio button 
                if (functions.IsChecked == true)
                {
                    WhichFunction();
                }
                if (healthyBehaviors.IsChecked == true)
                {
                    WhichHealthyBehavior();
                }
                if (unhealthyBehaviors.IsChecked == true)
                {
                    WhichUnhealthyBehavior();
                }
                //Hide other parts and make facts box and go button visible
                cerebellumBox.SetValue(Canvas.LeftProperty, originalCerebellumX);
                cerebellumBox.SetValue(Canvas.TopProperty, originalCerebellumY);
                temporalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                frontalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                occipitalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                parietalLobeBox.Visibility = System.Windows.Visibility.Hidden;
                scrambleButton.Visibility = System.Windows.Visibility.Hidden;
                resetButton.Visibility = System.Windows.Visibility.Hidden;
                cutoutUpstairsBrain.Visibility = System.Windows.Visibility.Hidden;
                factsMessageBox.Visibility = System.Windows.Visibility.Visible;
                gotItButton.Visibility = System.Windows.Visibility.Visible;
                brainPartsLabel.Visibility = System.Windows.Visibility.Visible;
                disabled = true;
            }
        }
        #endregion
        //All images use this functions as the "drop" part of their drag and drop operation
        private void Img_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
            Canvas canvas = img.Parent as Canvas;
            Mouse.OverrideCursor = null;
            movingObject = null;

            // Put the image currently being dragged on top of the others
            int top = Canvas.GetZIndex(img);
            foreach (System.Windows.Controls.Image child in canvas.Children)
                if (top > Canvas.GetZIndex(child))
                    top = Canvas.GetZIndex(child);
            Canvas.SetZIndex(img, top + 1);
            Mouse.Capture(null);
        }
        //All images use this function for their "drag" part of their drag and drop operation
        private void Img_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
            if (e.LeftButton == MouseButtonState.Pressed && sender == movingObject)
            {
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                Canvas canvas = img.Parent as Canvas;
                Mouse.OverrideCursor = null;
                double newLeft = e.GetPosition(canvas).X - firstXPos - canvas.Margin.Left;
                // newLeft inside canvas right-border?
                if (newLeft > canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth)
                {
                    newLeft = canvas.Margin.Left + canvas.ActualWidth - img.ActualWidth;
                    Mouse.OverrideCursor = Cursors.No;
                }
                // newLeft inside canvas left-border?
                else if (newLeft < canvas.Margin.Left)
                {
                    newLeft = canvas.Margin.Left;
                    Mouse.OverrideCursor = Cursors.No;
                }
                img.SetValue(Canvas.LeftProperty, newLeft);

                double newTop = e.GetPosition(canvas).Y - firstYPos - canvas.Margin.Top;
                // newTop inside canvas bottom-border?
                if (newTop > canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight)
                {
                    newTop = canvas.Margin.Top + canvas.ActualHeight - img.ActualHeight;
                    Mouse.OverrideCursor = Cursors.No;
                }
                // newTop inside canvas top-border?
                else if (newTop < canvas.Margin.Top)
                {
                    newTop = canvas.Margin.Top;
                    Mouse.OverrideCursor = Cursors.No;
                }
                img.SetValue(Canvas.TopProperty, newTop);

            }
        }

        //This function is to reset brain parts to their original position
        private void ResetImages(object sender, RoutedEventArgs e)
        {


            frontalLobeBox.SetValue(Canvas.LeftProperty, originalFrontalLobeX);
            frontalLobeBox.SetValue(Canvas.TopProperty, originalFrontalLobeY);
            parietalLobeBox.SetValue(Canvas.LeftProperty, originalParietalLobeX);
            parietalLobeBox.SetValue(Canvas.TopProperty, originalParietalLobeY);
            temporalLobeBox.SetValue(Canvas.LeftProperty, originalTemporalLobeX);
            temporalLobeBox.SetValue(Canvas.TopProperty, originalTemporalLobeY);
            occipitalLobeBox.SetValue(Canvas.LeftProperty, originalOccipitalLobeX);
            occipitalLobeBox.SetValue(Canvas.TopProperty, originalOccipitalLobeY);
            cerebellumBox.SetValue(Canvas.LeftProperty, originalCerebellumX);
            cerebellumBox.SetValue(Canvas.TopProperty, originalCerebellumY);

        }

        //This function is to randowmly place different brain parts within pictureCanvas
        private void ScrambleButton_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            frontalLobeBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(frontalLobeBox.ActualWidth))));
            frontalLobeBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(frontalLobeBox.ActualHeight))));
            parietalLobeBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(parietalLobeBox.ActualWidth))));
            parietalLobeBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(parietalLobeBox.ActualHeight))));
            temporalLobeBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(temporalLobeBox.ActualWidth))));
            temporalLobeBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(temporalLobeBox.ActualHeight))));
            occipitalLobeBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(occipitalLobeBox.ActualWidth))));
            occipitalLobeBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(occipitalLobeBox.ActualHeight))));
            cerebellumBox.SetValue(Canvas.LeftProperty, Convert.ToDouble(rnd.Next(0, 660 - Convert.ToInt32(cerebellumBox.ActualWidth))));
            cerebellumBox.SetValue(Canvas.TopProperty, Convert.ToDouble(rnd.Next(0, 375 - Convert.ToInt32(cerebellumBox.ActualHeight))));


        }
       

        //Depending on the brain part
        //This functions sets the autoreverse attribute 
        //of that brain part's storyboard false
        private void ReverseOff(int storyboardFlag)
        {
            switch (storyboardFlag)
            {
                case 1:
                    sbFL.AutoReverse = false;
                    break;
                case 2:
                    sbTL.AutoReverse = false;
                    break;
                case 3:
                    sbPL.AutoReverse = false;
                    break;
                case 4:
                    sbOL.AutoReverse = false;
                    break;
                case 5:
                    sbC.AutoReverse = false;
                    break;
            }
        }
        //Depending on the brain part
        //This functions sets the autoreverse attribute 
        //of that brain part's storyboard true
        private void ReverseOn(int storyboardFlag)
        {
            switch (storyboardFlag)
            {
                case 1:
                    sbFL.Begin(this, true);
                    sbFL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbFL.AutoReverse = true;
                    break;
                case 2:
                    sbTL.Begin(this, true);
                    sbTL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbTL.AutoReverse = true;
                    break;
                case 3:
                    sbPL.Begin(this, true);
                    sbPL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbPL.AutoReverse = true;
                    break;
                case 4:
                    sbOL.Begin(this, true);
                    sbOL.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbOL.AutoReverse = true;
                    break;
                case 5:
                    sbC.Begin(this, true);
                    sbC.Seek(this, new TimeSpan(0, 0, 0), TimeSeekOrigin.Duration);
                    sbC.AutoReverse = true;
                    break;
            }
        }
        //This function helps ensure the correct AutoReverse attribute of the selected brain part
        //Which brain parts to make visible based on brain part selection
        //Make the gotItbutton, facts message box, and brain parts label hidden
        private void GotItButton_Click(object sender, RoutedEventArgs e)
        {     //sbFL = 1, sbTL = 2, sbPL = 3, sbOL = 4, sbC = 5
            //Safeguard against incorrect AutoREverse attribute of selected brain part 
            if (sbFL.AutoReverse)
            { ReverseOn(1); }
            else { ReverseOff(1); }
            if (sbTL.AutoReverse)
            { ReverseOn(2); }
            else { ReverseOff(2); }
            if (sbPL.AutoReverse)
            { ReverseOn(3); }
            else { ReverseOff(3); }
            if (sbOL.AutoReverse)
            { ReverseOn(4); }
            else { ReverseOff(4); }
            if (sbC.AutoReverse)
            { ReverseOn(5); }
            else { ReverseOff(5); }

            //Based on the brain part selected, determines which parts
            //should become visible again
            switch (storyboardFlag)
            {
                case 1:
                    parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                    occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 2:
                    parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                    occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 3:
                    frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                    occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 4:
                    parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    cerebellumBox.Visibility = System.Windows.Visibility.Visible;
                    break;
                case 5:
                    parietalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    temporalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    frontalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    occipitalLobeBox.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
            resetButton.Visibility = System.Windows.Visibility.Visible;
            scrambleButton.Visibility = System.Windows.Visibility.Visible;
            factsMessageBox.Visibility = System.Windows.Visibility.Hidden;
            gotItButton.Visibility = System.Windows.Visibility.Hidden;
            brainPartsLabel.Visibility = System.Windows.Visibility.Hidden;
            cutoutUpstairsBrain.Visibility = System.Windows.Visibility.Visible;


            //Safegaurd against incorrect AutoReverse attribute of selected brain part 
            if (!sbFL.AutoReverse && storyboardFlag == 1)
            { ReverseOn(1); }
            else { ReverseOff(1); }
            if (!sbTL.AutoReverse && storyboardFlag == 2)
            {ReverseOn(2); }
            else { ReverseOff(2); }
            if (!sbPL.AutoReverse && storyboardFlag == 3)
            { ReverseOn(3); }
            else { ReverseOff(3); }
            if (!sbOL.AutoReverse && storyboardFlag == 4)
            { ReverseOn(4); }
            else { ReverseOff(4); }
            if (!sbC.AutoReverse && storyboardFlag == 5)
            { ReverseOn(5); }
            else { ReverseOff(5); }

       
            disabled = false;
            storyboardFlag = 0; //reset storyboard flag
            factsMessageBox.Text = "";
            functions.IsChecked = false;
            healthyBehaviors.IsChecked = false;
            unhealthyBehaviors.IsChecked = false;
        }
      
    }
}

