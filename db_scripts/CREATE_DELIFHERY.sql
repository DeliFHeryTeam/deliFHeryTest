CREATE TABLE address(
    street VARCHAR(20) NULL,
    city VARCHAR(20) NULL,
    country VARCHAR(20) NULL,
    houseNo INTEGER NULL,
    postalCode INTEGER NULL,
    addressID INTEGER NULL
);

ALTER TABLE address
    ADD PRIMARY KEY (addressID);

CREATE TABLE contact (
        contactID INTEGER NULL,
        senderID INTEGER NOT NULL
);

ALTER TABLE contact
    ADD PRIMARY KEY (contactID,senderID);

CREATE TABLE customer(
        username VARCHAR(20) NULL,
        authID VARCHAR(20) NULL,
        senderID INTEGER NOT NULL
);

ALTER TABLE customer
    ADD PRIMARY KEY (senderID);

CREATE TABLE email(
        email CHAR(18) NULL,
        contactID INTEGER NOT NULL,
        senderID INTEGER NOT NULL
);

ALTER TABLE email
    ADD PRIMARY KEY (contactID,senderID);

CREATE TABLE package(
        packageID INTEGER NULL,
        weightKg INTEGER NULL,
        lengthCm CHAR(18) NULL,
        widhtCm CHAR(18) NULL,
        heightCm CHAR(18) NULL
    );

ALTER TABLE package
    ADD PRIMARY KEY (packageID);

CREATE TABLE phonenumber(
        contactID INTEGER NOT NULL,
        senderID INTEGER NOT NULL,
        phonenumber CHAR(18) NULL
);

ALTER TABLE phonenumber
    ADD PRIMARY KEY (contactID,senderID);

CREATE TABLE postOffice(
        senderID INTEGER NOT NULL,
        name VARCHAR(20) NULL
);

ALTER TABLE postOffice
    ADD PRIMARY KEY (senderID);

CREATE TABLE sender(
        senderID INTEGER NULL,
        addressID INTEGER NULL
    );

ALTER TABLE sender
    ADD PRIMARY KEY (senderID);

CREATE TABLE shipmentOrder(
        createdAt DATE NULL,
        priceEur INTEGER NULL,
        trackingNo VARCHAR(20) NULL,
        packageID INTEGER NOT NULL,
        senderID INTEGER NOT NULL,
        addressID INTEGER NULL
    );

ALTER TABLE shipmentOrder
    ADD PRIMARY KEY (packageID,senderID);

CREATE TABLE trackingHistoryEntry(
        packageID INTEGER NOT NULL,
        statusCode VARCHAR(20) NULL,
        trackingHistoryEntryID INTEGER NULL,
        timeStamp DATE NULL,
        senderID INTEGER NOT NULL,
        addressID INTEGER NULL
    );

ALTER TABLE trackingHistoryEntry
    ADD PRIMARY KEY (packageID,trackingHistoryEntryID,senderID);

ALTER TABLE contact
    ADD FOREIGN KEY FK_Contact_Sender (senderID) REFERENCES customer(senderID);

ALTER TABLE customer
    ADD FOREIGN KEY ISA_Customer_Sender (senderID) REFERENCES sender(senderID)
    ON DELETE CASCADE;

ALTER TABLE email
    ADD FOREIGN KEY IS_A_Email_Contact (contactID,senderID) REFERENCES contact(contactID,senderID)
    ON DELETE CASCADE;

ALTER TABLE phonenumber
    ADD FOREIGN KEY IS_A_PhoneNr_Contact (contactID,senderID) REFERENCES contact(contactID,senderID)
    ON DELETE CASCADE;

    ALTER TABLE postOffice
        ADD FOREIGN KEY IS_A_Postoffice_Sender (senderID) REFERENCES sender(senderID)
    ON DELETE CASCADE;

    ALTER TABLE sender
    ADD FOREIGN KEY FK_Sender_Address(addressID) REFERENCES address(addressID)
    ;

    ALTER TABLE shipmentOrder
        ADD FOREIGN KEY FK_Order_Package(packageID) REFERENCES package(packageID);

    ALTER TABLE shipmentOrder
        ADD FOREIGN KEY FK_Order_Sender (senderID) REFERENCES sender(senderID);

    ALTER TABLE shipmentOrder
        ADD FOREIGN KEY FK_Order_Address_Receiver (addressID) REFERENCES address(addressID);

    ALTER TABLE shipmentOrder
        ADD FOREIGN KEY FK_Order_Addres_Sender (addressID) REFERENCES address(addressID);

    ALTER TABLE trackingHistoryEntry
    ADD FOREIGN KEY FK_History_Order (packageID,senderID) REFERENCES shipmentOrder(packageID,senderID)
    ;
    ALTER TABLE trackingHistoryEntry
    ADD FOREIGN KEY FK_History_Address (addressID) REFERENCES address(addressID);

