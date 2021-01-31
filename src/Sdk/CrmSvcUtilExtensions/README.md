#	Extensions for the Microsoft Dynamics 365 CE and PowerApps CrmSvcUtil tool

##	Summary

This package contains extensions for the **CrmSvcUtil** code generation tool contained in the **Microsoft Dynamics 365 CE** and **PowerApps** tools.
When you install the Nuget package the installation will require installation of the dependecy packages including of the CrmSvcUtil executable. You can use the tool to generate code for entities, attributes, option sets, etc. Before running the tool, adjust the configuration files according to your needs. Works well with both on-premise and online environments.

If you are not familiar with CrmSvcUtil read the below:

[Generate early-bound classes for the Organization service](https://docs.microsoft.com/en-us/powerapps/developer/common-data-service/org-service/generate-early-bound-classes)

[Create early bound entity classes with the code generation tool](https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/org-service/create-early-bound-entity-classes-code-generation-tool)

##	How to use it

1. Create a new .NET Framework class library project (preferably version 4.7.2).
2. Install the Nuget package and accept installing the dependecy packages.
3. Create a folder named "Domain" or choose another name of your preference under the project.
4. Edit and adjust the two **.config** files according to your needs.
5. Edit the **entities.xml** file with the entities to be included in the code generation.
6. Edit and adjust the **generate.bat** file according to your needs and environment settings.
7. Compile the project.
8. Open a command prompt and navigate to the project output binaries folder (I usually find handy of changing the binaries output folder to a single bin folder for all the build configurations).
9. Run the generate batch file, *e.g. generate yourusername yourpassword [yourdomain]*. You can also use the interactive login mode of CrmSvcUtil to connect/authenticate to your environment but you will need to add a reference to the **Microsoft.Xrm.Tooling.Ui.Styles.dll** in your project. The assembly can be found in the *coretools* folder.

Note: If you are generating separate files per type then you might be interested to edit the *.csproj* file manually to allow dynamic loading of all the *.cs* files in the *Domain* folder \(e.g. <Compile Include="Domain\\\*.cs" />\).

##	Release notes

####	Version - 19.5.1621

Changes:
- Rectified package issue with the .NET Framework 4.6.2 target

####	Version - 19.5.1421

Changes:
- Downgraded/targeting .NET Framework 4.6.2

####	Version - 19.5.120

Initial release contains the following features:
- Generates code for early bound entities
- Generates code for single-select option sets as enumerations fully integrated with the entity attributes
- Generates code for multi-select option sets as enumerations fully integrated with the entity attributes
- Supporting assembly compliance by using the **Humanizer** library and display names
- Reduces the size of the generated code by simplifying the namespaces on each type
- Option to generate all entities
- Option to generate separate files per type or a single file containing all types
- Option to generate either all or the referenced option sets by the generated entities only
- Option to generate setters for read-only attributes to help with mocking objects whilst implementing automated unit tests (should not be used in actual code)
- Option to define the suffix of primary key, status and status reason fields
- Option to define the prefix of the global option sets
- Option to define the maximum length of characters of entity, attribute or option set names
- Black list the entities that are to be excluded
- Black list the attributes that are to be excluded
- Define the entities to be generated
- Define the activity entities
- Does not generate entity relationships

