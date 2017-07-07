using UnityEngine;
using System.Collections;
using System;

namespace Playmove
{
    public class NamesRegisterButton : PYButton
    {
        [SerializeField]
        private PYText _nameHolder;

        [SerializeField]
        private NamesManagerPopup.PlayerInfo[] _players;
        public NamesManagerPopup.PlayerInfo[] Players
        {
            get { return _players; }
            private set { _players = value; }
        }

		public PlayerLogButton playerLogButton;

        protected override void Start()
        {
            base.Start();
            if (NamesManagerPopup.Instance != null)
            {
                NamesManagerPopup.Instance.ClearFilter();
            }
        }

        protected override void ClickAction()
        {
            base.ClickAction();

            if (string.IsNullOrEmpty(_nameHolder.Text))
            {
                if (Players.Length <= 1)
                    NamesManagerPopup.RegisterNames(NamesRegistred, Players[0]);
                else
                    NamesManagerPopup.RegisterNames(NamesRegistred, Players);
            }
            else
            {
                for (int i = 0; i < Players.Length; i++)
                {
                    NamesManagerPopup.Instance.RemoveFromFilter(Players[i].Name);
                    Players[i].Name = "";
                    Players[i].ClassId = 0;
                }
                _nameHolder.Text = string.Empty;
            }
        }

		public void ClickSimulation(){
			ClickAction ();
		}

        private void NamesRegistred(NamesManagerPopup.PlayerInfo[] names)
        {
            if (names != null && names.Length > 0)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] != null)
                    {
                        Players[i].Name = names[i].Name;
                        Players[i].ClassId = names[i].ClassId;
                        PYNamesManager.SaveName(Players[i].Name, Players[i].ClassId);
                    }
                    else
                    {
                        Players[i].Name = "Anônimo";
                        Players[i].ClassId = 0;
                    }
                    _nameHolder.Text += Players[i].ToString() + "\n";
					playerLogButton.Clicked (Players [i].Name, Players [i].ClassId, gameObject);
                }
            }
        }
    }
}