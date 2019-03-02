# Validation Library

In web development, we need to validate the data provided by user in multiple place. DotNet framework has introduced a library **System.ComponentModel.DataAnnotations** which provides some default validation attribute through which we can validate user data in one place and do not need to re-write the same rule in multiple places. 

This Validation Library provides custom data annotation apart from default validation  which is mentioned below in detail.

## Getting Started

The developer need to apply these custom attribute on class property which will help to validate model object. Install this nuget package to your application and start using the attribute.

The nuget package **ValidationLibrary.Attributes** is available in Conduent DevOps. 

In order to download the nuget, configure the Nuget Package Source with below address:
https://pkgs.dev.azure.com/ConduentDevOps/_packaging/CAP-Artifacts/nuget/v3/index.json

### Prerequisites

This nuget package can be consumed by any .net core v2.2 application.

## Validation Attribute Usage

**EnsureMinBaseOnFlag** - This attribute is applied on integer type property and ensures that the value set for the property should be greater than or equal to the specified value (minValueOnTrue) but only in the condition that the value set for attribute property (flagProperty) is set to true.

***Arguments:***

* *minValueOnTrue*: Property value should be atleast this value
* *flagProperty*: Name of the boolean property the value of which should be true to apply the validation
* *ErrorMessage*: Message to be displayed if the validation fails.

```
public class HotelModel
{
    [EnsureMinBaseOnFlag(minValueOnTrue:10, flagProperty:"RoomExists", 
        ErrorMessage = "Room Count should not be less than 10")]
    public object RoomCount { get; set; }
    public bool RoomExists { get; set; }
}
```
> When creating object of HotelModel, if value set for RoomExists property is true, then RoomCount should be ateast 10 or above.

**EnsureMinimumElement** - This attribute is applied on IList collection property which ensures that the collection count should be greater than or equal to the specified value (minElements).

***Arguments:***

* *minElements*: Property value specifies the minimum count of IList collection.
* *ErrorMessage*: Message to be displayed if the validation fails.

```
public class HotelModel
{
    [EnsureMinimumElements(minElements:10, 
        ErrorMessage = "Minimum 10 Rooms are required")]
    public IList<Room> Rooms { get; set; }
}
```
> When creating object of HotelModel, Rooms collection should have atleast 10 items in it.

**EnsureMinimumValue** - This attribute is applied on integer type property which ensures that the value set to the property should be greater than or equal to the specified value (minimumValue).

***Arguments:***

* *minimumValue*: Property value should be greater than or equal to minimumValue value set to attribute.
* *ErrorMessage*: Message to be displayed if the validation fails.

```
public class HotelModel
{
    [EnsureMinimumValue(minimumValue:100, 
        ErrorMessage = "Room number should start from 100")]
    public int RoomNumber { get; set; }
}
```
> When creating object of HotelModel, value set to RoomNumber should be greater than or equal to 100

**NotEmptyGuid** - This attribute is applied on Guid type property which ensures that the value set to the property should be a valid Guid. Empty Guid are not accepted.

***Arguments:***

* *ErrorMessage*: Message to be displayed if the validation fails.

```
public class HotelModel
{
    [NotEmptyGuid(ErrorMessage = "Room Id is not valid.")]
    public Guid RoomId { get; set; }
}
```
> When creating object of HotelModel, RoomId property should not be set to Empty Guid.

**RequiredIf** - This attribute can be applied on any type of property. It ensures that the property value should always set with some value if the first parameter (propertyName) has the same value as second parameter (desiredValue).

***Arguments:***

* *propertyName*: Name of the property which is checked with desired value
* *desiredValue*: Value of the property (propertyName)
* *ErrorMessage*: Message to be displayed if the validation fails.

```
public class RoomTypeModel
{
    [RequiredIf(propertyName: "IsExecutive", desiredValue: true, 
        ErrorMessage = "Additional Service charge is required")]
    public double ServiceCharges { get; set; }
    public bool IsExecutive { get; set; }
}
```
> When creating object of RoomTypeModel, if IsExecutive value is set to true, then ServiceCharges property is a required field.

**RequiredIfNot** - This attribute can be applied on any type of property. It ensures that the value set to the object property is required only if the value passed to it doesn't contain in the desire value list (DesiredValues) of specified property (PropertyName).

***Arguments:***

* *propertyName*: Name of the property which is checked with desired value
* *desiredValue*: Value of the property (propertyName). It could be either single value or collection of values
* *ErrorMessage*: Message to be displayed if the validation fails.

```
public class RoomTypeModel
{
    [RequiredIfNot(propertyName:"RoomType", 
        desiredValue: new object[] { "Single", "Double", "Economy" }, 
        ErrorMessage = "Service charge is required")]
    public double ServiceCharge { get; set; }
    public object RoomType { get; set; }
}
```
> When creating object of RoomType, if the RoomType is not of type single, double, economy, then additional service charge is required field.

```
public class RoomTypeModel
{
    [RequiredIfNot(propertyName:"RoomType", 
        desiredValue: "Economy" }, 
        ErrorMessage = "Service charge is required")]
    public double ServiceCharge { get; set; }
    public object RoomType { get; set; }
}
```
> When creating object of RoomType, if the RoomType is not of economy type, then service charge is required field.

## Declaration
This library is intended to use within Conduent Service.
