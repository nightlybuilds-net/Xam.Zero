
  
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

NB: Xamarin Zero will register ***all Pages and ViewModels in bootstrap time***. This will be possible only if every view model will extends the ZeroBaseModel class. The registration is Singleton by default; to make it multi-instance style, we have to use the [Transient](https://github.com/markjackmilian/Xam.Zero/blob/develop/Xam.Zero/Xam.Zero/Classes/TransientAttribute.cs) attribute.

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

2. If the page is independent from the Shell, for example after a page push in the navigation stack, just the view model location is requested.
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
```

#### ZeroApp
ZeroApp helps us using builder pattern for bootstrapping our application.
```csharp
ZeroApp  On(Application  app){}
ZeroApp  WithContainer(IContainer  container){}
ZeroApp  RegisterShell<T>(Func<T> shell) where  T : Shell{}
void  Start(){}
void  StartWith<T>() where  T : Shell{}
```

#### IShellService
it is responsible of switching navigation containers. Default use case is change from a login status to a logout one.
```csharp
void  SwitchContainer<T>() where  T : Shell{}
```


### IoC Container
Xamarin.Zero does not force you to use a specific IoC container, however it offers you an implementation through [Xam.Zero.DryIoC](https://github.com/markjackmilian/Xam.Zero/tree/develop/Xam.Zero/Xam.Zero.DryIoc) nuget. [DryIoC](https://github.com/dadhi/DryIoc) is a fast and lean IoC container for .NET. Otherwise you can always use one of your favourite implementations.

### IMessagingCenter
During bootstrapping, on application start up, Xamarin.Forms [IMessagingCenter](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.imessagingcenter?view=xamarin-forms) Interface will be registered, so that we can inject it in view models constructor implementing Dependency Injection. This does not preclude the use of the standard Xamarin.Forms singleton mechanism.

## Samples

 - Simple/Tabbed navigation app by switching navigation containers; 
 - Custom IoC containers;
