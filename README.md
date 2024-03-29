

  
# Welcome to Xamarin.Zero

![enter image description here](https://raw.githubusercontent.com/Gaburiere/Xam.Zero/develop/xam.zero_logo.jpg)

Hi! I'm [Mark Jack Milian](http://markjackmilian.net/) and i'm here to aswer a few questions


## Packages ##


Platform/Feature               | Package name                              | Stable      | Status 
-----------------------|-------------------------------------------|-----------------------------|------------------------
Core             | `Xam.Zero.Sem` | [![Nuget](https://img.shields.io/nuget/v/Xam.Zero.Sem)](https://www.nuget.org/packages/Xam.Zero.Sem/) | [![Build Status](https://dev.azure.com/nightlybuilds-net/Xam.Zero/_apis/build/status/markjackmilian.Xam.Zero?branchName=master)](https://dev.azure.com/nightlybuilds-net/Xam.Zero/_build/latest?definitionId=11&branchName=master)|
Core  | `Xam.Zero.DryIoc.Sem`  | [![NuGet](https://img.shields.io/nuget/v/Xam.Zero.DryIoc.Sem)](https://www.nuget.org/packages/Xam.Zero.Sem/) | [![Build Status](https://dev.azure.com/nightlybuilds-net/Xam.Zero/_apis/build/status/markjackmilian.Xam.Zero?branchName=master)](https://dev.azure.com/nightlybuilds-net/Xam.Zero/_build/latest?definitionId=11&branchName=master)|

All packages are compliant with [Semantic Versioning](https://semver.org/)





## What is Xamarin.Zero?

A simple, easy, agile and fluent framework for Xamarin forms which encapsulates all the features of Xamarin by supporting developers with a real MVVM architecture. With Xamarin.Zero you will be able to build up the structure of your app in Zero seconds with no waste of time!

## Why Xamarin.Zero?

With the continuous and more frequent evolution of Xamarin, it has become necessary having a framework that supports the structure and functionality of the new Shell component, available from Xamarin.Forms 4.

## How does it works?

### Initialisation and navigation containers
Please add a reference of our nuget (inserire link al nuget) to your Cross-Platform Xamarin.Forms project.

As usual, we have to set the start up point of our application and, like in Xamarin.Forms Vanilla, we can do it by settings a value of MainPage. the fluency of Xamarin Zero permit us to set IoC container, navigation containers and theirs order in one row only:

```csharp
public partial class App : Application
{
	public static readonly Container Container = new Container();

	public App()
	{
		this.InitializeComponent();
		
		ZeroApp.On(this)
			.WithContainer(DryIocZeroContainer.Build(Container))
			.RegisterShell(() => new AppShell())
			.RegisterShell(() => new TabbedShell())
			.StartWith<AppShell>();
	}
	...
}
```
Where:
- ***Container*** is our favourite implementation of an IoC Container;
- ***ZeroApp***  is a wrapper around Xamarin.Forms bootstrapping the main Application;
- ***AppShell*** and ***TabbedShell*** are two sample shells which act as navigation containers. Please see ***Samples*** section for further details.

NB: Xamarin Zero will register ***all Pages and ViewModels in bootstrap time***. This will be possible only if every view model will extends the ZeroBaseModel class. The registration is Singleton by default (you can choose for transient using Attribute or builder method); to make it multi-instance style, we have to use the [Transient](https://github.com/markjackmilian/Xam.Zero/blob/develop/Xam.Zero/Xam.Zero/Classes/TransientAttribute.cs) attribute.

### Binding context and markup
there are two mandatory ways to define the binding context of out pages and both use a [markup extension mechanism](https://github.com/markjackmilian/Xam.Zero/tree/develop/Xam.Zero/Xam.Zero/MarkupExtensions):

 1. If the page is within a Shell, like in a tabbed or master-detail navigation, we have to specify to the markup both the view and view model locations.

```csharp
<?xml version="1.0" encoding="utf-8"?>
<ContentPage 
	x:Name="Home"
	xmlns="http://xamarin.com/schemas/2014/forms"
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
	xmlns:markupExtensions="clr-namespace:Xam.Zero.MarkupExtensions;assembly=Xam.Zero"
	xmlns:home="clr-namespace:Xam.Zero.Dev.Features.Home;assembly=Xam.Zero.Dev"
	BindingContext="{markupExtensions:ShellPagedViewModelMarkup ViewModel={x:Type home:HomeViewModel}, Page={x:Reference Home}}"
	x:Class="Xam.Zero.Dev.Features.Home.HomePage"> 

	...
</ContentPage>
```

Where ViewModel is the type of the ***Viewmodel*** we want to bind to the view and ***Page*** is a reference to the ContentPage

2. If the page is independent from the Shell, for example after a page push in the navigation stack, you can use markup extension method with just the view model location is requested.
Or you can use **binding by convention**: if no BindingContext with Markup is defined Xam.Zero will look for a ViewModel that is called like: "PageName"+ViewModel.
So You can just push a SecondPage and Zero will try to bind with a SecondPageViewModel (nice!). 
**NB ViewModel type must be defined on the same assembly of Page** 
```csharp
<?xml version="1.0" encoding="utf-8"?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
	xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
	xmlns:markupExtensions="clr-namespace:Xam.Zero.MarkupExtensions;assembly=Xam.Zero"
	xmlns:second="clr-namespace:Xam.Zero.Dev.Features.Second;assembly=Xam.Zero.Dev"
	BindingContext="{markupExtensions:ViewModelMarkup ViewModel={x:Type second:SecondViewModel}}"
	x:Class="Xam.Zero.Dev.Features.Second.SecondPage">

	...
</ContentPage>
```
the markup extension will advise Xamarin Zero about the binding between view and viewModel, making the binding and navigation mechanism very simple to manage.

### Navigation between pages
As said before, the navigation is very easy after markup definition; we just have to make our view model inherit from [ZeroBaseModel](https://github.com/markjackmilian/Xam.Zero/blob/develop/Xam.Zero/Xam.Zero/ViewModels/ZeroBaseModel.cs), in order to gain all the signature we want for navigating (and not only)


```csharp
Task Push<T>(object data = null, bool animated = true) where T: Page {}
Task PushModal<T>(object data = null, bool animated = true) where T: Page {}
Task Pop(object data = null, bool animated = true) {}
Task PopModal(object data = null, bool animated = true) {}
```

### Passing data during navigation
 With the useful signatures Push and Pop, we can pass data as object parameters. This data are trapped within the Init() and ReverseInit() of ZeroBaseModel, in which we can cast to a type we want to use.
 
### Zero Command
This is introduced in version 1.1.0 and is a custom implementation of ICommand with many useful features.
the goal is to keep the ViewModel as clean as possible by **automatically tracking dependencies** and composing a flow related to the execution of the command.
You can find usage examples in "CommandPageViewModel" in this repo.

You can create instances of ICommand using the ZeroComandBuilder and you can customize the flow of the activity in a descriptive way.

#### Dependency tracker
When you create a zerocommand you have to specify an INotifyPropertyChanged instance (usually the viewmodel) so that the CanExecute expression is evaluated by finding properties that exist on the viewmodel in order to re-evaluate the canexecute automatically.
Example:

     this.TestSuccessCommand = ZeroCommand.On(this)
                .WithCanExecute(()=> !this.IsBusy && !string.IsNullOrEmpty(this.Name))
                .WithExecute((commandParam, context) => this.InnerShowMessageAction())
                .Build();

So CanExecute on this ICommand is automatically evaluated when IsBusy or SomeProperty changed (all tracked dependencies must be implement propertychanged)

You can add a dependency on a observablecollection using ".WithRaiseCanExecuteOnCollectionChanged", when the collection changed the canexecute expressio will be evaluated again.

**Error Handler**

Is possible to intercept exceptions in order to keep the executor method as clean as possible:

      this.TestErrorCommand = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam, context) => this.InnerManageErrorWithoutSwallow())
                .WithErrorHandler(exception => base.DisplayAlert("Managed Exception", exception.Message, "ok"))
                .Build();

**Before and After Execute**

Is possibile to run some logic before execute and after execute. If before execute return false it will stop the execution.


            this.BeforeRunEvaluationCommadn = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam, context) => this.InnerEvaluateCanRun())
                .WithBeforeExecute(context => base.DisplayAlert("Before Run Question", "Can i run?", "yes", "no"))
                .WithAfterExecute(context =>
                    base.DisplayAlert("I'm running after a execution", "I'll not run if evaluation fail!", "ok"))
                .Build();


You can pass data between flow step (before, execute, after) using context:

    this.ContextEvaluationCommand = ZeroCommand.On(this)
                .WithCanExecute(this.InnerExpression())
                .WithExecute((commandParam, context) => this.InnerShowMessageAction())
                .WithBeforeExecute(context =>
                {
                    var stopWatch = new Stopwatch();
                    context.Add(stopWatch);
                    stopWatch.Start();
                    return true;
                })
                .WithAfterExecute(async context =>
                {
                    var stopWatch = context.Get<Stopwatch>();
                    stopWatch.Stop();
                    await this.DisplayAlert("Evaluation", $"Executed in {stopWatch.ElapsedMilliseconds} ms", "OK");
                    stopWatch.Reset();
                })
                .Build();

**Auto Invalidate Command**

ZeroCommand can autoinvalidate itself during execution so it auto prevent double-tap button.

    this.AutoInvalidateCommand = ZeroCommand.On(this)
                .WithAutoInvalidateWhenExecuting()
                .WithExecute(async (o, context) =>
                {
                    await Task.Delay(1000);
                    await base.DisplayAlert("Auto invalidate", "Now button should be disabled!", "ok");
                }).Build();

**Concurrent Execution**

You can set how many concurrent execution could support:
`.WithConcurrentExecutionOf(3)`
where 3 is the number of concurrent execution (default value is 1)


### Useful Signatures
Xamarin Zero offers many useful signature awesomely designed to satisfy our MVVM desire!

#### ZeroBaseModel
PreviousModel and CurrentPage help us organizing data
```csharp
ZeroBaseModel  PreviousModel { get; set; }
Page  CurrentPage { get; set; }
```
PrepareModel() and ReversePrepareModel() are called after a page push and pop from the navigation stack. Complete control is achieved by Appearing and Disappearing event handlers support.
```csharp
virtual  void  CurrentPageOnDisappearing(object  sender, EventArgs  e){}
virtual  void  CurrentPageOnAppearing(object  sender, EventArgs  e){}
virtual  Task  PrepareModel(object  data){}
virtual  Task  ReversePrepareModel(object  data){}
```

A full display managing is guaranteed.
```csharp
Task<bool> DisplayAlert(string  title, string  message, string  accept, string  cancel){}
Task  DisplayAlert(string  title, string  message, string  cancel){}
Task<string> DisplayActionSheet(string title, string cancel, string destruction, string[] buttons){}
Task<string> DisplayPrompt(string title, string message, string accept = "OK", string cancel = "Cancel", string placeholder = null, int maxLength = -1, Keyboard keyboard = null, string initialValue = "")
```

#### ZeroApp
ZeroApp helps us using builder pattern for bootstrapping our application.
```csharp
ZeroApp  On(Application  app){}
ZeroApp  WithContainer(IContainer  container){}
ZeroApp  RegisterShell<T>(Func<T> shell) where  T : Shell{}
void  Start(){}
void  StartWith<T>() where  T : Shell{}
ZeroApp WithTransientViewModels()  
ZeroApp WithTransientPages()
```
Default lifecycle for pages and viewmodels is singleton. You can register all as transient using WithTransientViewModels/Pages, or you can use [Transient] attribute.

#### IShellService
it is responsible of switching navigation containers. Default use case is change from a login status to a logout one.
```csharp
void  SwitchContainer<T>() where  T : Shell{}
```


### IoC Container
Xamarin.Zero does not force you to use a specific IoC container, however it offers you an implementation through [Xam.Zero.DryIoC](https://github.com/markjackmilian/Xam.Zero/tree/develop/Xam.Zero/Xam.Zero.DryIoc) nuget. [DryIoC](https://github.com/dadhi/DryIoc) is a fast and lean IoC container for .NET. Otherwise you can always use one of your favourite implementations.

### IMessagingCenter
During bootstrapping, on application start up, Xamarin.Forms [IMessagingCenter](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.imessagingcenter?view=xamarin-forms) Interface will be registered, so that we can inject it in view models constructor implementing Dependency Injection. This does not preclude the use of the standard Xamarin.Forms singleton mechanism.

### Weak Event Handler Pattern
Xam.Zero gracefully implements this pattern, by whom you can manage more efficiently navigation events. In the case you can subscribe to CurrentPageOnAppeanring and CurrentPageOnDisappearing events and even extend them by adding more login in your ViewModel, without worrying about unsubscribe to them. The WeakEventHandler class is a public sealed one in Xam.Zero, so you can use it for managing every kind of event.

## Samples

 - Simple/Tabbed navigation app by switching navigation containers; 
 - Custom IoC containers;
