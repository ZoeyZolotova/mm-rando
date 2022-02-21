#ifndef _MACRO_H_
#define _MACRO_H_

#define FIELD_OFFSET(Type, FieldName) ((u32)(&((Type*)0)->FieldName))
#define FIELD_TO_STRUCT(Type, FieldName, FieldPtr) ((Type*)((u32)(FieldPtr) - FIELD_OFFSET(Type, FieldName)))
#define FIELD_TO_STRUCT_CONST(Type, FieldName, FieldPtr) ((const Type*)(FIELD_TO_STRUCT(Type, FieldName, FieldPtr)))

#endif
