**SEED - Sintesizer for Enterprise Experimentation Data**

This is a product generator for realistic enterprise data. The generated gizontograph is based on structural indicatiors that can be retrieved by analysing real enterprise data or estimated for wide range of experiments. Operations are created and distributed over machines based on a transition matrix that is able to shift the organizational degree seemlesly between 0 (Job Shop) and 1 (Flow shop).

Answered questions:
- [x] Lambda required ? (id does almost nothing but determing from which side the procedure is coming close to the target value )
    Yes it is, it determines the shift of the target column of the generated transition Matrix.
- [x] Randomizing required ? (implemented but not required.)
    Yes to create different, operations with same degree of organization


ToDo:
Questions to answer:
- [ ] Start for Flowshop always M1 ? (it will create a static amount of operations n that is equal to amout of resources for all materials) 
- [ ] Forbid to jump from source to sink ? 
- [ ] Implement Transition Matrix with source and sink


