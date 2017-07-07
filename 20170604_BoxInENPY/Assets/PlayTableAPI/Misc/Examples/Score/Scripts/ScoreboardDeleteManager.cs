using UnityEngine;
using System.Collections;

namespace Playmove
{
    public class ScoreboardDeleteManager : MonoBehaviour
    {
        public ScoreboardWindow ScoreboardRef;
        public PYButton DeleteButton;

        private PYLocalization Localization
        {
            get
            {
                return PYBundleManager.Localization;
            }
        }

        //private bool onButtonVoltar = false;
        private bool onButtonContinuar = false;

        void Start()
        {
            DeleteButton.onClick.AddListener((sender) => DeleteAll());
        }

        public void DeleteAll()
        {
            DeleteButton.IsEnabled = false;

            PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_DeleteMsg01).Play();

            PYAlertPopup popup = PYAlertPopup.InvokeAlertPopup("AlertPopup_2b")
                .SetTitle(Localization.GetAsset<string>(TagManager.LOCALIZATION_ATENTION, "ATENÇÂO") + "!")
                .SetText(Localization.GetAsset<string>(TagManager.LOCALIZATION_RESTRICTED_ACCESS, "ACESSO RESTRITO A RESPONSÁVEIS."));
            popup.UseFader = true;

            popup.AddButton("PYButtonText", Localization.GetAsset<string>(TagManager.LOCALIZATION_BACK, "VOLTAR")).AddButtonAction(0, PlayBackSound);
            popup.AddButton("PYButtonText", Localization.GetAsset<string>(TagManager.LOCALIZATION_CONTINUE, "CONTINUAR")).AddButtonAction(1, () =>
                {
                    onButtonContinuar = true;
                    popup.CloseFaderByPopup = false;
                });
            popup.Open();

            popup.onClosed.AddListener(() =>
                {
                    if (onButtonContinuar)
                    {
                        onButtonContinuar = false;
                        ConfirmDelete();
                    }
                    else
                    {
                        DeleteButton.IsEnabled = true;
                    }
                });
        }

        private void ConfirmDelete()
        {
            PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_Continuar).Play();
            PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_DeleteMsg02).Delay(1f).Play();

            PYAlertPopup popup = PYAlertPopup.InvokeAlertPopup("AlertPopup_2b")
                .SetTitle(Localization.GetAsset<string>(TagManager.LOCALIZATION_ATENTION, "ATENÇÂO") + "!")
                .SetText(Localization.GetAsset<string>(TagManager.LOCALIZATION_CONFIRM_DELETE_ALL, "DESEJA MESMO APAGAR O PLACAR \nDESTE JOGO ?"));
            popup.UseFader = true;
            popup.AddButton("PYButtonText", Localization.GetAsset<string>(TagManager.LOCALIZATION_BACK, "VOLTAR")).AddButtonAction(0, PlayBackSound);
            popup.AddButton("PYButtonText", Localization.GetAsset<string>(TagManager.LOCALIZATION_CONTINUE, "CONTINUAR")).AddButtonAction(1, () =>
                {
                    onButtonContinuar = true;
                    popup.CloseFaderByPopup = false;
                });
            popup.Open();

            popup.onClosing.AddListener(() =>
            {
                if (PYAudioManager.Instance != null)
                    PYAudioManager.Instance.Stop(PYAudioTags.Voice_ptBR_DeleteMsg02);
            });
            popup.onClosed.AddListener(() =>
            {
                if (onButtonContinuar)
                {
                    onButtonContinuar = false;
                    ClearScore();
                }
                else
                {
                    DeleteButton.IsEnabled = true;
                }
            });
        }

        private void ClearScore()
        {
            if (PYAudioManager.Instance != null)
            {
                PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_Continuar).Play();
                PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_DeleteMsg03).Delay(1f).Play();
            }

            PYScoreData.DeleteAll();

            ScoreboardRef.Refresh();

            PYAlertPopup popup = PYAlertPopup.InvokeAlertPopup("AlertPopup_1b")
                .SetTitle(Localization.GetAsset<string>(TagManager.LOCALIZATION_ATENTION, "ATENÇÂO") + "!")
                .SetText(Localization.GetAsset<string>(TagManager.LOCALIZATION_SCORE_DELETED, "O PLACAR FOI APAGADO."));
            popup.UseFader = true;
            popup.AddButton("PYButtonConfirm", null);
            popup.Open();

            popup.onClosing.AddListener(() =>
            {
                if (PYAudioManager.Instance != null)
                    PYAudioManager.Instance.Stop(PYAudioTags.Voice_ptBR_DeleteMsg03);
            });
            popup.onClosed.AddListener(() =>
            {
                ScoreMenuManager.Instance.ChangeScene("menu");
            });
        }

        private void PlayBackSound()
        {
            if (PYAudioManager.Instance != null)
                PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_Voltar).Play();
        }
    }
}