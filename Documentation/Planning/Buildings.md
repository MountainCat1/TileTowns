Buildings provide work slots, which can be filled with pops. Only work slot with a pop utilizing it produces income, amenities etc. (basically like in stellaris)

* Houses - provide housing and amenities 
* Farm - provides income (*food* or *currency*) + a little bit of housing
* Wood mill - provides income (wood or *currency*)
* Church - provides amenities and control

### Work Slots
* Farmer {
	  income = 1
	  salary = 0.7
  }
* House Keeper {
	  salary = 0.7
	  amenities = 1 
  }
* Priest {
	  salary = 3
	  amenities = 1 
  }

Building

```csharp
{
	string Name;
	Dictionary<Workslot, int> Workslots;
} 
```

Workslot
```json
{
	decimal Salary;
	decimal Income;
}
```