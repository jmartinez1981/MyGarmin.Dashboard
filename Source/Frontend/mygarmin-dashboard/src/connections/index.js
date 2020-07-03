import * as React from "react";
import { List, Datagrid, TextField, SimpleForm, BooleanField, DateField, PasswordInput, TextInput, required } from 'react-admin';
import { Edit } from 'react-admin';
import LoadDataButton from './loadDataButton';
import StravaAuthButton from './stravaAuthButton';

export const StravaConnectionList = props => (
    <List {...props}>
        <Datagrid rowClick="edit">
            <TextField label="Type" source="connectionType" />
            <TextField label="Id" source="id" />
            <BooleanField label="Data Loaded" source="isDataLoaded" />
            <DateField label="Last update" source="lastUpdate" />            
            <StravaAuthButton/>
            <LoadDataButton />
        </Datagrid>
    </List>
);

export const StravaConnectionEdit = (props) => (
    <Edit {...props}>
        <SimpleForm>
            <TextInput disabled label="Id" source="id" />
            <PasswordInput label="Token" source="token" validate={required()} />
            <PasswordInput label="Refresh token" source="refreshToken" validate={required()} />
        </SimpleForm>
    </Edit>
);