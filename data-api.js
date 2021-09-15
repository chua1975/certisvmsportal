apiAc = {
    ////this header is very important 
    ////or else browser return you 'CORS' error !!!
    header:  function () {
        let header = {};
        header['Accept'] = 'application/json';
        header['Content-Type'] = 'application/json';
        return header;
    },
    baseUrlAc:  function () {
        return GetBaseUrl() + "/api/ac";
    },
    baseUrlCheckin: function () {
        return GetBaseUrl() + "/api/checkin";
    },
    fingerApi: function() {
        return "http://localhost:8090/gms/";
    },
    source: "Web Portal",


    checkVisit:  function (idNo) {
        return fetch(apiAc.baseUrlAc() + '/checkVisit',
        {
            method: "POST",
            headers:  apiAc.header(),
            body: JSON.stringify({ IC: idNo, Source: apiAc.source })
        }).then(resp => resp.json());
    },
    movement: function (profileId, isStaff) {
        return fetch(apiAc.baseUrlAc() + '/movement',
        {
            method: "POST",
            headers: apiAc.header(),
            body: JSON.stringify({ ProfileId: profileId, IsStaff:isStaff })
        }).then(resp => resp.json());
    },
    getFingerData: function () {
        return new Promise((ok, exp) => {
            $.ajax({
                "type":"get",
                "url": apiAc.fingerApi()+"reg",
                "success": res => {
                    ok(res);
                },
                "error": err => {
                    exp(err);
                }
            });
        });
    },
    compareFingerData: function (left, right, captured) {
        return new Promise((ok, exp) => {
            $.ajax({
                "type": "post",
                "url": apiAc.fingerApi() + "compare",
                "data": JSON.stringify(
                    {
                        CapturedFinger: captured,
                        Finger1: left,
                        Finger2: right
                    }),
                "success": res => {
                    ok(res);
                },
                "error": err => {
                    exp(err);
                }
            });
        });
    },
    regFinger:  function(idNo, finger1, finger2) {
        return fetch(apiAc.baseUrlAc() + '/regFinger',
            {
                method: "POST",
                headers:  apiAc.header(),
                body: JSON.stringify({ IC: idNo, FingerPrint1: finger1, FingerPrint2:finger2})
            }).then(resp => resp.json());
    },
    regPass:  function (idNo, regId) {
        return fetch(apiAc.baseUrlAc() + '/regPass',
            {
                method: "POST",
                headers:  apiAc.header(),
                body: JSON.stringify({ Barcode: idNo, RegId:regId })
            }).then(resp => resp.json());
    },

    checkinValidate:  function (idNo) {
        return fetch(apiAc.baseUrlCheckin() + '/validate',
            {
                method: "POST",
                headers:  apiAc.header(),
                body: JSON.stringify({ IC: idNo, Source: apiAc.source })
            }).then(resp => resp.json());
    },
    checkinValidateQr:  function (idNo) {
        return fetch(apiAc.baseUrlCheckin() + '/validateQr',
            {
                method: "POST",
                headers:  apiAc.header(),
                body: JSON.stringify({ QrContent: idNo})
            }).then(resp => resp.json());
    },
    checkinSaveState:  function (regId) {
        return fetch(apiAc.baseUrlCheckin() + '/saveState',
            {
                method: "POST",
                headers:  apiAc.header(),
                body: JSON.stringify({ RegId: regId })
            }).then(resp => resp.json());
    },
    updatePhoto:  function(profileId, photo) {
        return fetch(apiAc.baseUrlCheckin() + '/updatePic',
            {
                method: "POST",
                headers:  apiAc.header(),
                body: JSON.stringify({ VisitorID: profileId, Photo: photo })
            }).then(resp => resp.json());
    }
};