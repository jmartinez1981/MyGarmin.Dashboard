import * as React from "react";
import { List, Datagrid, TextField, SimpleForm, BooleanField, DateField, PasswordInput, TextInput, required } from 'react-admin';
import { Edit } from 'react-admin';
import { Create } from 'react-admin';
import LoadDataButton from './loadDataButton';

export const StravaConnectionList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField label="ClientId" source="id" />
            <BooleanField label="Data Loaded" source="isDataLoaded" />
            <DateField label="Last update" source="lastUpdate" />
            <LoadDataButton />
        </Datagrid>
    </List>
);

export const StravaConnectionEdit = (props) => (
    <Edit {...props}>
        <SimpleForm>
            <TextInput disabled label="ClientId" source="id" />
            <PasswordInput label="Token" source="token" validate={required()} />
            <PasswordInput label="Refresh token" source="refreshToken" validate={required()} />
        </SimpleForm>
    </Edit>
);

export const StravaConectionCreate = props => (
    <Create {...props}>
        <SimpleForm>
            <TextInput label="ClientId" source="id" validate={required()} />
            <PasswordInput label="Token" source="token" validate={required()} />
            <PasswordInput label="Refresh token" source="refreshToken" validate={required()} />
        </SimpleForm>
    </Create>
);