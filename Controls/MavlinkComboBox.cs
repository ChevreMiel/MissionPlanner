﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace ArdupilotMega.Controls
{
    public class MavlinkComboBox : ComboBox
    {
        public new event EventHandler SelectedIndexChanged;

        [System.ComponentModel.Browsable(true)]
        public string ParamName { get; set; }

        [System.ComponentModel.Browsable(true)]
        public Hashtable param { get; set; }

        Control _control;
        Type _source;
        string paramname2 = "";

        public MavlinkComboBox()
        {
            this.Enabled = false;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public void setup(Type source, string paramname, Hashtable paramlist, string paramname2 = "", Control enabledisable = null)
        {
            base.SelectedIndexChanged -= MavlinkComboBox_SelectedIndexChanged;

            _source = source;

            this.DataSource = Enum.GetNames(source);

            this.ParamName = paramname;
            this.param = paramlist;
            this.paramname2 = paramname2;
            this._control = enabledisable;

            if (paramlist.ContainsKey(paramname))
            {
                this.Enabled = true;
                this.Visible = true;

                enableControl(true);

                this.Text = Enum.GetName(source, (Int32)(float)paramlist[paramname]);
            }

            base.SelectedIndexChanged += new EventHandler(MavlinkComboBox_SelectedIndexChanged);
        }

        void enableControl(bool enable)
        {
            if (_control != null)
                _control.Enabled = enable;
        }

        void MavlinkComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
                this.SelectedIndexChanged(sender,e);

            if (!MainV2.comPort.setParam(ParamName, (float)(Int32)Enum.Parse(_source, this.Text)))
            {
                CustomMessageBox.Show("Set " + ParamName + " Failed!");
            }

            if (paramname2 != "")
            {
                if (!MainV2.comPort.setParam(paramname2, (float)(Int32)Enum.Parse(_source, this.Text) > 0 ? 1: 0))
                {
                    CustomMessageBox.Show("Set " + paramname2 + " Failed!");
                }
            }
        }
    }
}
