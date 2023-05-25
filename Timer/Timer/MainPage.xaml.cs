using System;
using Plugin.Maui.Audio;

namespace Timer;

public partial class MainPage : ContentPage
{
    private bool _ısItRunnin;
    private int _secoundtbx;
    private int _minutestbx;
    private int _hourstbx;
    private bool _isIsStart = false;
    private List<TimeSpan> _listofTimeSpans = new();
    private TimeSpan _timrSpan;

    private readonly IAudioManager _audioManager;
    private bool _soundIsOver;


    public MainPage(IAudioManager audioManager)
	{
		InitializeComponent();
        this._audioManager = audioManager;

    }

    private async void OnStartImageButtonClicked(object sender, EventArgs e)
    {

        try
        {
            _ısItRunnin = !_ısItRunnin;
            _secoundtbx = Convert.ToInt16(Secoundstbx.Text);
            _minutestbx = Convert.ToInt16(Minutestbx.Text);
            _hourstbx = Convert.ToInt16(Hourstbx.Text);

            Start.Source = _ısItRunnin ? "pause.png" : "play.png";

            _timrSpan = new TimeSpan(Convert.ToInt16(Hourstbx.Text), Convert.ToInt16(Minutestbx.Text),
                Convert.ToInt16(Secoundstbx.Text));

            if (_isIsStart == false)
            {

                _listofTimeSpans.Add(_timrSpan);

                _isIsStart = true;
            }

            if (_ısItRunnin && _timrSpan.TotalSeconds != 0)
            {
                TuskManegerbtx.IsEnabled = false;
            }
            else
            {
                TuskManegerbtx.IsEnabled = true;
            }

            while (_ısItRunnin && _timrSpan.TotalSeconds != 0)
            {

                _timrSpan = _timrSpan.Add(TimeSpan.FromSeconds(-1));
                Secoundstbx.Text = _timrSpan.Seconds.ToString();
                Minutestbx.Text = _timrSpan.Minutes.ToString();
                Hourstbx.Text = _timrSpan.Hours.ToString();
                await Task.Delay(TimeSpan.FromSeconds(1));

                if (_timrSpan.TotalMinutes <= 0 && _isIsStart)
                {
                    var playsound = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("Sound.mp3"));

                    playsound.Play();
                    playsound.Loop = true;

                    var display = await DisplayAlert("The Time İs Over!!!", " The time is over. Do you wanna stop the alarm?", "ok", "please immediately");


                    if (display)
                    {
                        playsound.Stop();
                        playsound.Loop = false;

                        _ısItRunnin = !_ısItRunnin;
                        Start.Source = _ısItRunnin ? "pause.png" : "play.png";
                        _listofTimeSpans.Clear();
                    }
                    else
                    {
                        playsound.Stop();
                        playsound.Loop = false;

                        _ısItRunnin = !_ısItRunnin;
                        Start.Source = _ısItRunnin ? "pause.png" : "play.png";
                        _listofTimeSpans.Clear();
                    }
                }
            }
        }

        catch
        {

        }
    }

    private void OnRestartImageButtonClicked(object sender, EventArgs e)
    {

        try
        {
            _timrSpan = new TimeSpan(_listofTimeSpans[0].Hours, _listofTimeSpans[0].Minutes, _listofTimeSpans[0].Seconds);
            Secoundstbx.Text = _timrSpan.Seconds.ToString();
            Minutestbx.Text = _timrSpan.Minutes.ToString();
            Hourstbx.Text = _timrSpan.Hours.ToString();
        }

        catch
        {

        }
    }

    private void OnStopImageButtonClicked(object sender, EventArgs e)
    {
        try
        {
            _timrSpan = new TimeSpan(0, 0, 0);
            Secoundstbx.Text = _timrSpan.Seconds.ToString();
            Minutestbx.Text = _timrSpan.Minutes.ToString();
            Hourstbx.Text = _timrSpan.Hours.ToString();
            _isIsStart = false;
            _listofTimeSpans.Clear();
            _isIsStart = !_ısItRunnin;
            if (Start.Source != ImageSource.FromResource("play.png"))
            {
                Start.Source = "play.png";
            }
        }

        catch
        {

        }
    }

}

