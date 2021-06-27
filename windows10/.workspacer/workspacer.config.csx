//replace C:\workspacer with the path you installed it on (default is C:\Program Files\workspacer)
//open this in Visual Studio 2019, VS Code doesn't like the Lambda expression below.
#r "C:\workspacer\workspacer.Shared.dll"
#r "C:\workspacer\plugins\workspacer.Bar\workspacer.Bar.dll"
#r "C:\workspacer\plugins\workspacer.ActionMenu\workspacer.ActionMenu.dll"
#r "C:\workspacer\plugins\workspacer.FocusIndicator\workspacer.FocusIndicator.dll"

using System;
using System.Linq;
using System.IO;
using workspacer;
using workspacer.Bar;
using workspacer.Bar.Widgets;
using workspacer.ActionMenu;
using workspacer.FocusIndicator;

Action<IConfigContext> doConfig = (IConfigContext context) =>
{
    context.AddBar(new BarPluginConfig()
    {
        BarTitle = "workspacer.Bar",
        FontSize = 14,
        FontName = "JetBrainsMono NF",
        RightWidgets = () => new IBarWidget[] { new TimeWidget(), new BatteryWidget() },
    });

    context.AddFocusIndicator(new FocusIndicatorPluginConfig()
    {
        BorderColor = Color.Lime,
        TimeToShow = 150,
    });

    var actionMenu = context.AddActionMenu(new ActionMenuPluginConfig()
    {
        Foreground = Color.Blue,
    });

    var sticky = new StickyWorkspaceContainer(context, StickyWorkspaceIndexMode.Local);
    context.WorkspaceContainer = sticky;
    // create workspaces
    sticky.CreateWorkspaces("Web", "Code", "Chat", "Media", "Extra");

    context.WindowRouter.AddRoute((window) => window.Title.Contains("Visual Studio") ? context.WorkspaceContainer["Code"] : null);
    context.WindowRouter.AddRoute((window) => window.Title.Contains("Alacritty") ? context.WorkspaceContainer["Code"] : null);

    context.WindowRouter.AddRoute((window) => window.Title.Contains("Spotify") ? context.WorkspaceContainer["Media"] : null);
    context.WindowRouter.AddRoute((window) => window.Title.Contains("Stremio") ? context.WorkspaceContainer["Media"] : null);

    context.WindowRouter.AddRoute((window) => window.Title.Contains("Waterfox") ? context.WorkspaceContainer["Web"] : null);

    context.WindowRouter.AddRoute((window) => window.Title.Contains("Discord") ? context.WorkspaceContainer["Chat"] : null);
    context.WindowRouter.AddRoute((window) => window.Title.Contains("Zoom") ? context.WorkspaceContainer["Chat"] : null);

    // filters, workspacer will ignore windows with this names (I recommend ignoring games and fullscreen applications)
    context.WindowRouter.AddFilter((window) => !window.Title.Contains("Getsu")); //Getsu Fuma Den
    context.WindowRouter.AddFilter((window) => !window.Title.Contains("Lacuna")); //Lacuna Noir
    context.WindowRouter.AddFilter((window) => !window.Title.Contains("Monster Train")); //Monster Train

    // keybinds
    KeyModifiers mod = KeyModifiers.Alt;

    // default keybindings: https://github.com/rickbutton/workspacer/blob/master/src/workspacer/Keybinds/KeybindManager.cs

    //unsuscribe defaults that conflict with other software or that I don't use
    context.Keybinds.Unsubscribe(mod, Keys.O); //I'm mapping Alt + O to open PowerToys Run
    context.Keybinds.Unsubscribe(mod | KeyModifiers.LShift, Keys.O); //not going to use
    context.Keybinds.Unsubscribe(mod | KeyModifiers.LShift, Keys.I); //not going to use

    //suscribe
    context.Keybinds.Subscribe(mod | KeyModifiers.LShift, Keys.G, () => System.Diagnostics.Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)));
    //context.Keybinds.Subscribe(mod | KeyModifiers.LShift, Keys.Enter, () => System.Diagnostics.Process.Start(Environment.ExpandEnvironmentVariables(@"%ProgramFiles%\Alacritty\alacritty.exe"));
};

return doConfig;