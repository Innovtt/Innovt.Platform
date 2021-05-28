# Dynamo DB Strategy for Authorization Database

<!-- TOC -->

- [Dynamo DB Strategy for Authorization Database](#dynamo-db-strategy-for-authorization-database)
  - [Access Pattern](#access-pattern)
  - [Design your primary keys and secondary indexes](#design-your-primary-keys-and-secondary-indexes)
    - [Index](#index)
    - [Inverted Index](#inverted-index)
    - [Composite Sort Key](#composite-sort-key)
  - [Filtering access pattern](#filtering-access-pattern)

<!-- /TOC -->

## Access Pattern
*	Get all groups
*   Get all roles
*   Get a group´s role
*	Get groups by user.
*   Get permissions by group
*   Get permission by role
*	Get permissions by user (main)

## Design your primary keys and secondary indexes

Model/Key  | PK  | SK 
--|---|--
Permission | P#123456 | Resource
Group  |  G#Admin |  
User   | 
Role   |  R#Admin | P#123456


### Index

PK  | SK  |  Attributes
--|---|--
USER#MICHEL  | PROFILE#MICHEL  |  
USER#MICHEL  | COMPANY#123  |  
USER#MICHEL  | COMPANY#1234  |  
USER#WELBERT  | COMPANY#123  |  

### Inverted Index

GSI)

SK  | PK  |  Attributes
--|---|--
PROFILE#MICHEL  |  USER#MICHEL |
COMPANY#123 | USER#MICHEL   |  
COMPANY#1234 | USER#MICHEL   |  
COMPANY#123  | USER#WELBERT |

### Composite Sort Key

Nothing

## Filtering access pattern

Access  | Query  
--|--
Get user profile  |  “PK= USER#michel@antecipa.com AND SK=”#PROFILE#antecipa”
Get contacts for user  |  “PK= USER#michel@antecipa.com AND SK=”#PROFILE#antecipa”
Get companies for user  |  “PK= USER#michel@antecipa.com AND BEGINS_WITH(SK,”COMPANY#”)”
Get users by company (GSI1)  |  “PK= COMPANY#123 “
Get user permission by company  |  “PK= USER#michel@antecipa.com AND SK= ”COMPANY#123”