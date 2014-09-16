namespace MWS.WindowsService
{
    partial class MovilizerWindowsService
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceTimer = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.serviceTimer)).BeginInit();
            // 
            // serviceTimer
            // 
            this.serviceTimer.AutoReset = false;
            // 
            // MovilizerWindowsService
            // 
            this.ServiceName = "MovilizerWindowsService";
            ((System.ComponentModel.ISupportInitialize)(this.serviceTimer)).EndInit();

        }

        #endregion

        private System.Timers.Timer serviceTimer;
    }
}
