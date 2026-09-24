# How to use templates

**Keywords** and explanation :

- Chapter props : The chapter properties, what makes each chapter unique, and how the program writes it.
- X parts : How many parts "sub-chapters" do that chapter have.
- from X : Start counting parts from X instead of normal counting (1).
- marked : Either this chapter and it's sub-chapters should be marked as done or not.

**Defining types** :

- Individual : A
- Range : [A, B]
- Groups : A, B, C, D

## How to define chapter props.

### Individual (Simplest) :

```txt
A : X parts from Y
```

This translates to : **Chapter A will have X sub-chapters counting from Y**.
This defines the props for 1 chapter only.

### Example :

```txt
3 : 1 parts from 1
```

## Groups :

```txt
A, B, C : X parts from Y
```

This translates to : **Chapter A and B and C will have X sub-chapters counting from Y**.
This defines the props for multiple chapters only.

### Example :

```txt
2, 4, 6 : 6 parts from 3
```

## Range :

```txt
[A, B] : X parts from Y
```

This translates to : **Chapters from A to B and between will have X sub-chapters counting from Y**.

### Example :

```txt
[1, 4] : 1 parts from 2 marked
```

### Notes :

- Templates support overriding by the last define, so for example if you defined a range between [1, 10] and wanted to treat chapter 5 specially, you can define chapter 5 individually, for instance :

```txt
[1, 4] : 1 parts from 2 marked
3 : 1 parts from 1
```

The app will write from 1 to 4 normally (1 part, counting from 2, marked as done) but when it comes to writing chapter 3 it will write it based on the last define for it (1 part, counting from 1, unmarked).

- Templates **ALWAYS** overwrite existing chapter files.
- You can only use (file-name, title, sub-title, output, sub-chapter-style, numbering-style) cli options when using templates.
- It's still under development, many features are coming in the way
